using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.Configuration;
using SnapToTable.Infrastructure.DTOs;
using System.Text.Json;
using System.Text.RegularExpressions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Microsoft.Extensions.Options;

namespace SnapToTable.Infrastructure.Services;

public class RecipeExtractionService : IRecipeExtractionService
{
    private readonly IOpenAIService _client;
    private readonly IOptions<OpenAiSettings> _options;

    public RecipeExtractionService(IOpenAIService client, IOptions<OpenAiSettings> options)
    {
        _client = client;
        _options = options;
        ;
    }

    public async Task<IReadOnlyList<RecipeExtractionResult>> GetRecipeFromImagesAsync(IEnumerable<ImageInput> images,
        CancellationToken cancellationToken)
    {
        var request = await GetChatCompletionCreateRequestAsync(images, cancellationToken);
        var rawRecipes = await GetRawRecipesAsync(request, cancellationToken);

        var recipeExtractionResults = rawRecipes.Recipes.Select(rawRecipe => new RecipeExtractionResult
        {
            Name = rawRecipe.Name,
            Category = rawRecipe.Category,
            Servings = ParseInt(rawRecipe.Servings),
            PrepTime = ParseTimeSpan(rawRecipe.PrepTime),
            CookTime = ParseTimeSpan(rawRecipe.CookTime),
            AdditionalTime = ParseTimeSpan(rawRecipe.AdditionalTime),
            Ingredients = rawRecipe.Ingredients,
            Directions = rawRecipe.Directions,
            Notes = rawRecipe.Notes
        });

        return recipeExtractionResults.ToArray();
    }

    private async Task<RawRecipesDto> GetRawRecipesAsync(ChatCompletionCreateRequest request,
        CancellationToken cancellationToken)
    {
        var completion = await _client.ChatCompletion.CreateCompletion(request, cancellationToken: cancellationToken);

        if (!completion.Successful)
        {
            var error = completion.Error?.Message ?? "Unknown error.";
            throw new InvalidOperationException($"Failed to get a successful response from OpenAI: {error}");
        }

        var jsonResponse = completion.Choices.First().Message.Content;

        if (jsonResponse is null)
        {
            throw new InvalidOperationException($"Failed to get a successful json response from OpenAI");
        }

        var rawRecipe = JsonSerializer.Deserialize<RawRecipesDto>(jsonResponse, JsonSerializerConfiguration.Options);

        if (rawRecipe == null)
        {
            throw new InvalidOperationException("Failed to deserialize the recipe from OpenAI response.");
        }

        return rawRecipe;
    }

    private async Task<ChatCompletionCreateRequest> GetChatCompletionCreateRequestAsync(IEnumerable<ImageInput> images,
        CancellationToken cancellationToken)
    {
        var chatMessages = new List<ChatMessage>();
        var systemChatMessage = new ChatMessage(StaticValues.ChatMessageRoles.System, new List<MessageContent>
        {
            new()
            {
                Type = "text",
                Text =
                    "You are a helpful recipe assistant. Extract up to 3 recipes for a full meal from the attached image(s). If information is spread across multiple images, combine it intelligently. If the image(s) doesn't contain enough ingredients for any meal recipes return empty for recipes property. Return the data as a single JSON object. The JSON object must have this schema : ```json\n{\n  \"recipes\": [\n    {\n      \"name\": \"The full name of the recipe\",\n      \"category\": \"e.g., Dessert, Main Course, Appetizer\",\n      \"prepTime\": \"e.g., '15 minutes'\",\n      \"cookTime\": \"e.g., '10 minutes'\",\n      \"additionalTime\": \"e.g., '1 hour chilling' or empty string\",\n      \"servings\": \"e.g., '4'\",\n      \"ingredients\": [\n        \"full ingredient string 1\",\n        \"full ingredient string 2\"\n      ],\n      \"directions\": [\n        \"step 1\",\n        \"step 2\"\n      ],\n      \"notes\": [\n        \"note 1\",\n        \"note 2\"\n      ]\n    }\n  ]\n}```\n. Note that no fields can be empty."
            }
        });
        chatMessages.Add(systemChatMessage);
        
        var userMessageItems = new List<MessageContent>();

        foreach (var imageInput in images)
        {
            using var memoryStream = new MemoryStream();
            await imageInput.Content.CopyToAsync(memoryStream, cancellationToken);
            var imageBytes = memoryStream.ToArray();
            var base64Image = Convert.ToBase64String(imageBytes);
            var imageBase64Url = $"data:{imageInput.ContentType};base64,{base64Image}";
            userMessageItems.Add(new MessageContent
            {
                Type = "image_url",
                ImageUrl = new MessageImageUrl
                {
                    Url = imageBase64Url
                }
            });
        }

        chatMessages.Add(new ChatMessage(StaticValues.ChatMessageRoles.User, userMessageItems));

        var request = new ChatCompletionCreateRequest
        {
            Messages = chatMessages,
            Model = _options.Value.Model,
            MaxTokens = _options.Value.Token,
            ResponseFormat = new() { Type = "json_object" }
        };
        return request;
    }


    private int? ParseInt(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var match = Regex.Match(input, @"\d+");
        if (match.Success && int.TryParse(match.Value, out var result))
        {
            return result;
        }

        return null;
    }

    private TimeSpan? ParseTimeSpan(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var totalMinutes = 0;
        var hourMatches = Regex.Matches(input, @"(\d+)\s*(hour|hr)");
        var minuteMatches = Regex.Matches(input, @"(\d+)\s*(minute|min)");

        if (hourMatches.Count > 0 && int.TryParse(hourMatches[0].Groups[1].Value, out int hours))
        {
            totalMinutes += hours * 60;
        }

        if (minuteMatches.Count > 0 && int.TryParse(minuteMatches[0].Groups[1].Value, out int minutes))
        {
            totalMinutes += minutes;
        }

        // Fallback for if it just returns a number, assume minutes.
        if (totalMinutes == 0 && int.TryParse(Regex.Match(input, @"\d+").Value, out int minutesOnly))
        {
            totalMinutes = minutesOnly;
        }

        return totalMinutes > 0 ? TimeSpan.FromMinutes(totalMinutes) : null;
    }
}