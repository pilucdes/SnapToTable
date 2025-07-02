using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.Configuration;
using SnapToTable.Infrastructure.DTOs;
using System.Text.Json;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Microsoft.Extensions.Options;
using SnapToTable.Infrastructure.Constants;
using SnapToTable.Infrastructure.Exceptions;
using SnapToTable.Infrastructure.Mappers;

namespace SnapToTable.Infrastructure.Services;

public class AiRecipeExtractionService : IAiRecipeExtractionService
{
    private readonly IOpenAIService _client;
    private readonly IOptions<OpenAiSettings> _options;

    public AiRecipeExtractionService(IOpenAIService client, IOptions<OpenAiSettings> options)
    {
        _client = client;
        _options = options;
    }

    public async Task<IReadOnlyList<RecipeExtractionResult>> GetRecipeFromImagesAsync(IEnumerable<ImageInput> images,
        CancellationToken cancellationToken)
    {
        var request = await BuildChatCompletionCreateRequestAsync(images, cancellationToken);
        var contentJson = await FetchCompletionContentAsync(request, cancellationToken);
        var rawRecipes = ParseRawRecipes(contentJson);

        return rawRecipes.Recipes.Select(RecipeMapper.ToExtractionResult).ToArray();
    }
    
    private async Task<ChatCompletionCreateRequest> BuildChatCompletionCreateRequestAsync(
        IEnumerable<ImageInput> images,
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
        ImageInput imageInput, CancellationToken cancellationToken)
    {
        using var memoryStream = new MemoryStream();
        await imageInput.Content.CopyToAsync(memoryStream, cancellationToken);
        var base64Image = Convert.ToBase64String(memoryStream.ToArray());

        return new MessageContent
        {
            Type = OpenAiConstants.ContentTypeImageUrl,
            ImageUrl = new MessageImageUrl
            {
                Url = $"data:{imageInput.ContentType};base64,{base64Image}"
            }
        };
    }
}