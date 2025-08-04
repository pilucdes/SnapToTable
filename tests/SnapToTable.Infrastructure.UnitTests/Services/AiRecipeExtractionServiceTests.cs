using System.Net;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.ImageResponseModel;
using Microsoft.Extensions.Logging;
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
    private readonly Mock<ILogger<AiRecipeExtractionService>> _loggerMock;
    private readonly AiRecipeExtractionService _service;

    public AiRecipeExtractionServiceTests()
    {
        var settings = AiRecipeExtractionDataFactory.CreateOpenAiSettings();
        var optionsMock = Options.Create(settings);

        _openAiServiceMock = new Mock<IOpenAIService>();
        _loggerMock = new Mock<ILogger<AiRecipeExtractionService>>();
        _service = new AiRecipeExtractionService(_loggerMock.Object, _openAiServiceMock.Object, optionsMock);
    }

    [Fact]
    public async Task GetRecipeFromImagesAsync_WithValidInputAndSuccessfulApiResponse_ShouldReturnMappedRecipes()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };
        var validJson = AiRecipeExtractionDataFactory.CreateValidRecipesJson();
        const string imageUrl = "https://localhost/image123.webp";

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


        var imageResponse = new ImageCreateResponse()
        {
            HttpStatusCode = HttpStatusCode.OK,
            Results =
            [
                new()
                {
                    Url = imageUrl,
                }
            ]
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        _openAiServiceMock
            .Setup(c => c.Image.CreateImage(It.IsAny<ImageCreateRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageResponse);

        // Act
        var result = await _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldHaveSingleItem();

        var recipe = result.First();
        recipe.Name.ShouldBe("Test Cake");
        recipe.Category.ShouldBe("Dessert");
        recipe.Url.ShouldBe(imageUrl);
        recipe.PrepTime.ShouldBe(TimeSpan.FromMinutes(10));
        recipe.CookTime.ShouldBe(TimeSpan.FromMinutes(30));
        recipe.AdditionalTime.ShouldBe(TimeSpan.FromMinutes(5));
        recipe.Servings.ShouldBe(8);
        recipe.Ingredients.ShouldBe(["Flour", "Sugar"]);
        recipe.Directions.ShouldBe(["Mix", "Bake"]);
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

    [Fact]
    public async Task GetRecipeFromImagesAsync_WhenImageGenerationFailsWithError_ShouldLogAndContinue()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };
        var validJson = AiRecipeExtractionDataFactory.CreateValidRecipesJson();
        const string errorMessage = "Image generation failed due to policy violation.";

        var chatApiResponse = new ChatCompletionCreateResponse
        {
            Choices = [new() { Message = new ChatMessage(StaticValues.ChatMessageRoles.Assistant, validJson) }]
        };

        var imageApiResponse = new ImageCreateResponse
        {
            Results = [],
            Error = new Error { MessageObject = errorMessage }
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(chatApiResponse);

        _openAiServiceMock
            .Setup(c => c.Image.CreateImage(It.IsAny<ImageCreateRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(imageApiResponse);

        // Act
        var result = await _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldHaveSingleItem();
        result.First().Url.ShouldBeEmpty();
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Failed to get image result. {errorMessage}")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetRecipeFromImagesAsync_WhenImageGenerationThrowsException_ShouldLogAndContinue()
    {
        // Arrange
        var imageInputs = new List<ImageInputDto> { AiRecipeExtractionDataFactory.CreateImageInput([1, 2, 3]) };
        var validJson = AiRecipeExtractionDataFactory.CreateValidRecipesJson();
        var exception = new HttpRequestException("Network error");

        var chatApiResponse = new ChatCompletionCreateResponse
        {
            Choices = [new() { Message = new ChatMessage(StaticValues.ChatMessageRoles.Assistant, validJson) }]
        };

        _openAiServiceMock
            .Setup(c => c.ChatCompletion.CreateCompletion(It.IsAny<ChatCompletionCreateRequest>(), null,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(chatApiResponse);

        _openAiServiceMock
            .Setup(c => c.Image.CreateImage(It.IsAny<ImageCreateRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _service.GetRecipeFromImagesAsync(imageInputs, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldHaveSingleItem();
        result.First().Url.ShouldBeEmpty();

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to generate image for recipe: Test Cake")),
                exception, // Assert that the exact exception instance was logged
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}