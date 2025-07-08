using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.Exceptions;
using SnapToTable.Infrastructure.Services;
using SnapToTable.Infrastructure.UnitTests.Factories;
using Xunit;

namespace SnapToTable.Infrastructure.UnitTests.Services;

public class AiRecipeExtractionServiceTests
{
    private readonly Mock<IOpenAIService> _openAiServiceMock;
    private readonly AiRecipeExtractionService _service;

    public AiRecipeExtractionServiceTests()
    {
        var settings = AiRecipeExtractionDataFactory.CreateOpenAiSettings();
        var optionsMock = Options.Create(settings);

        _openAiServiceMock = new Mock<IOpenAIService>();
        _service = new AiRecipeExtractionService(_openAiServiceMock.Object, optionsMock);
    }

    [Fact]
    public async Task GetRecipeFromImagesAsync_WithValidInputAndSuccessfulApiResponse_ShouldReturnMappedRecipes()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };
        var validJson = AiRecipeExtractionDataFactory.CreateValidRecipesJson();

        var apiResponse = new ChatCompletionCreateResponse
        {
            Choices =
            [
                new()
                {
                    Message = new ChatMessage(StaticValues.ChatMessageRoles.Assistant, validJson)
                }
            ]
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldHaveSingleItem();

        var recipe = result.First();
        recipe.Name.ShouldBe("Test Cake");
        recipe.Category.ShouldBe("Dessert");
        recipe.PrepTime.ShouldBe(TimeSpan.FromMinutes(10));
        recipe.CookTime.ShouldBe(TimeSpan.FromMinutes(30));
        recipe.AdditionalTime.ShouldBe(TimeSpan.FromMinutes(5));
        recipe.Servings.ShouldBe(8);
        recipe.Ingredients.ShouldBe(["Flour", "Sugar"]);
        recipe.Directions.ShouldBe(["Mix","Bake"]);
        recipe.Notes.ShouldContain("Enjoy");


        _openAiServiceMock.Verify(
            c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetRecipeFromImagesAsync_WhenApiCallFails_ShouldThrowOpenAiApiException()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };

        var failedResponse = new ChatCompletionCreateResponse
        {
            Error = new Error { MessageObject = "API key invalid." }
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(failedResponse);

        // Act & Assert
        var exception = await Should.ThrowAsync<OpenAiApiException>(() =>
            _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None));

        exception.Message.ShouldContain("API key invalid.");
    }

    [Fact]
    public async Task GetRecipeFromImagesAsync_WhenApiResponseHasNoContent_ShouldThrowOpenAiApiException()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };

        // Setup a successful response but with null content
        var successWithNoContent = new ChatCompletionCreateResponse
        {
            Choices = [new() { Message = new ChatMessage(StaticValues.ChatMessageRoles.Assistant, string.Empty) }]
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(successWithNoContent);

        // Act & Assert
        var exception = await Should.ThrowAsync<OpenAiApiException>(() =>
            _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None));

        exception.Message.ShouldContain("contained no content");
    }

    [Fact]
    public async Task
        GetRecipeFromImagesAsync_WhenApiResponseIsMalformedJson_ShouldThrowRecipeDeserializationException()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };
        var malformedJson = "{\"recipes\":[{\"name\":\"Test Cake\""; // Missing closing brackets

        var apiResponse = new ChatCompletionCreateResponse
        {
            Choices = [new() { Message = new ChatMessage(StaticValues.ChatMessageRoles.Assistant, malformedJson) }]
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act & Assert
        var exception = await Should.ThrowAsync<RecipeDeserializationException>(() =>
            _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None));

        exception.Message.ShouldContain("JSON was malformed");
        exception.Message.ShouldContain(malformedJson);
    }

    [Fact]
    public async Task GetRecipeFromImagesAsync_WhenApiResponseIsNull_ShouldThrowRecipeDeserializationException()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };
        const string nullJson = "null";

        var apiResponse = new ChatCompletionCreateResponse
        {
            Choices = [new() { Message = new ChatMessage(StaticValues.ChatMessageRoles.Assistant, nullJson) }]
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act & Assert
        var exception = await Should.ThrowAsync<RecipeDeserializationException>(() =>
            _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None));

        exception.Message.ShouldContain("resolved to null");
    }
}