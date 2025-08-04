using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.Configuration;
using SnapToTable.Infrastructure.DTOs;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SnapToTable.Infrastructure.Constants;
using SnapToTable.Infrastructure.Exceptions;
using SnapToTable.Infrastructure.Mappers;

namespace SnapToTable.Infrastructure.Services;

public class AiRecipeExtractionService : IAiRecipeExtractionService
{
    private readonly ILogger<AiRecipeExtractionService> _logger;
    private readonly IOpenAIService _client;
    private readonly IOptions<OpenAiSettings> _options;

    public AiRecipeExtractionService(ILogger<AiRecipeExtractionService> logger, IOpenAIService client,
        IOptions<OpenAiSettings> options)
    {
        _logger = logger;
        _client = client;
        _options = options;
    }

    public async Task<IReadOnlyList<RecipeExtractionResultDto>> GetRecipeFromImagesAsync(
        IEnumerable<ImageInputDto> images,
        CancellationToken cancellationToken)
    {
        var request = await BuildChatCompletionCreateRequestAsync(images, cancellationToken);
        var contentJson = await FetchCompletionContentAsync(request, cancellationToken);
        var rawRecipes = ParseRawRecipes(contentJson);

        await AugmentRecipesWithImageUrl(rawRecipes, cancellationToken);

        return rawRecipes.Recipes.Select(RecipeMapper.ToExtractionResult).ToArray();
    }

    private async Task AugmentRecipesWithImageUrl(RawRecipesDto rawRecipes, CancellationToken cancellationToken)
    {
        var tasks = rawRecipes.Recipes.Select(async recipe =>
        {
            try
            {
                var imageResult = await _client.Image.CreateImage(new ImageCreateRequest
                {
                    Prompt = string.Format(_options.Value.RecipeImageExtractionPrompt, recipe.Name,
                        string.Join(",", recipe.Ingredients)),
                    N = 1,
                    Size = _options.Value.ImageSize,
                    ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
                    Model = _options.Value.ImageModel,
                }, cancellationToken);

                if (imageResult is { Successful: false, Error: not null })
                {
                    _logger.LogError($"Failed to get image result. {imageResult.Error.Message}");
                    return;
                }
                
                recipe.Url = imageResult.Results.First().Url;
          
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate image for recipe: {RecipeName}", recipe.Name);
            }
        });

        await Task.WhenAll(tasks);
    }

    private async Task<ChatCompletionCreateRequest> BuildChatCompletionCreateRequestAsync(
        IEnumerable<ImageInputDto> images,
        CancellationToken cancellationToken)
    {
        var systemMessage =
            new ChatMessage(StaticValues.ChatMessageRoles.System, _options.Value.RecipeExtractionPrompt);

        var imageContentTasks = images.Select(image => CreateImageMessageContentAsync(image, cancellationToken));
        var userMessageContent = await Task.WhenAll(imageContentTasks);

        var userMessage = new ChatMessage(StaticValues.ChatMessageRoles.User, userMessageContent);

        return new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage> { systemMessage, userMessage },
            Model = _options.Value.Model,
            MaxTokens = _options.Value.Token,
            ResponseFormat = new() { Type = OpenAiConstants.ResponseFormatJson }
        };
    }

    private async Task<string> FetchCompletionContentAsync(ChatCompletionCreateRequest request,
        CancellationToken cancellationToken)
    {
        var completion = await _client.ChatCompletion.CreateCompletion(request, cancellationToken: cancellationToken);

        if (!completion.Successful)
        {
            var error = completion.Error?.Message ?? "Unknown error.";
            throw new OpenAiApiException($"Failed to get a successful response from OpenAI: {error}");
        }

        var choice = completion.Choices.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(choice?.Message.Content))
        {
            throw new OpenAiApiException("OpenAI response was successful but contained no content.");
        }

        return choice.Message.Content;
    }

    private RawRecipesDto ParseRawRecipes(string jsonContent)
    {
        try
        {
            var rawRecipes =
                JsonSerializer.Deserialize<RawRecipesDto>(jsonContent, JsonSerializerConfiguration.Options);

            return rawRecipes ?? throw new RecipeDeserializationException(
                "Failed to deserialize the recipe from OpenAI response. The JSON content resolved to null.");
        }
        catch (JsonException)
        {
            throw new RecipeDeserializationException(
                $"Failed to deserialize the recipe from OpenAI response. The JSON was malformed. Content: {jsonContent}");
        }
    }

    private static async Task<MessageContent> CreateImageMessageContentAsync(
        ImageInputDto imageInputDto, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await imageInputDto.Content.CopyToAsync(memoryStream, cancellationToken);
        var base64Image = Convert.ToBase64String(memoryStream.ToArray());

        return new MessageContent
        {
            Type = OpenAiConstants.ContentTypeImageUrl,
            ImageUrl = new MessageImageUrl
            {
                Url = $"data:{imageInputDto.ContentType};base64,{base64Image}"
            }
        };
    }
}