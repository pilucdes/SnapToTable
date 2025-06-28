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
        var rawRecipes = await GetRawRecipesFromApiAsync(request, cancellationToken);
        
        return rawRecipes.Recipes.Length == 0 ? [] : rawRecipes.Recipes.Select(RecipeMapper.ToExtractionResult).ToArray();
    }

    private async Task<RawRecipesDto> GetRawRecipesFromApiAsync(ChatCompletionCreateRequest request,
        CancellationToken cancellationToken)
    {
        var completion = await _client.ChatCompletion.CreateCompletion(request, cancellationToken: cancellationToken);

        if (!completion.Successful)
        {
            var error = completion.Error?.Message ?? "Unknown error.";
            throw new OpenAiApiException($"Failed to get a successful response from OpenAI: {error}");
        }

        var choice = completion.Choices.FirstOrDefault();
        if (choice?.Message.Content is null)
        {
            throw new OpenAiApiException("OpenAI response was successful but contained no content.");
        }

        var rawRecipes =
            JsonSerializer.Deserialize<RawRecipesDto>(choice.Message.Content, JsonSerializerConfiguration.Options);

        return rawRecipes ?? throw new RecipeDeserializationException(
            "Failed to deserialize the recipe from OpenAI response. The response might be null or malformed.");
    }

    private async Task<ChatCompletionCreateRequest> BuildChatCompletionCreateRequestAsync(
        IEnumerable<ImageInput> images,
        CancellationToken cancellationToken)
    {
        var systemMessage =
            new ChatMessage(StaticValues.ChatMessageRoles.System, _options.Value.RecipeExtractionPrompt);

        var userMessageContent = new List<MessageContent>();
        
        foreach (var image in images)
        {
            userMessageContent.Add(await CreateImageMessageContentAsync(image, cancellationToken));
        }

        var userMessage = new ChatMessage(StaticValues.ChatMessageRoles.User, userMessageContent);

        return new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage> { systemMessage, userMessage },
            Model = _options.Value.Model,
            MaxTokens = _options.Value.Token,
            ResponseFormat = new() { Type = OpenAiConstants.ResponseFormatJson }
        };
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