using Moq;
using Shouldly;
using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;
using Xunit;
using RecipeAnalysisRequestEntity = SnapToTable.Domain.Entities.RecipeAnalysisRequest;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;

public class CreateRecipeAnalysisRequestCommandHandlerTests
{
    private readonly Mock<IRecipeAnalysisRequestRepository> _repositoryMock;
    private readonly Mock<IAiRecipeExtractionService> _aiServiceMock;
    private readonly CreateRecipeAnalysisRequestCommandHandler _handler;

    public CreateRecipeAnalysisRequestCommandHandlerTests()
    {
        _repositoryMock = new Mock<IRecipeAnalysisRequestRepository>();
        _aiServiceMock = new Mock<IAiRecipeExtractionService>();
        _handler = new CreateRecipeAnalysisRequestCommandHandler(_repositoryMock.Object, _aiServiceMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCallDependencies()
    {
        // Arrange
        var command = RecipeAnalysisDataFactory.CreateValidCommand();
        var aiResult = RecipeAnalysisDataFactory.CreateRecipeExtractionResults();

        _aiServiceMock
            .Setup(s => s.GetRecipeFromImagesAsync(command.Images, It.IsAny<CancellationToken>()))
            .ReturnsAsync(aiResult);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _aiServiceMock.Verify(s => s.GetRecipeFromImagesAsync(command.Images, It.IsAny<CancellationToken>()),
            Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<RecipeAnalysisRequestEntity>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCorrectlyMapAiResultToEntity()
    {
        // Arrange
        var command = RecipeAnalysisDataFactory.CreateValidCommand();
        var aiResult = RecipeAnalysisDataFactory.CreateRecipeExtractionResults();

        _aiServiceMock
            .Setup(s => s.GetRecipeFromImagesAsync(It.IsAny<List<ImageInput>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aiResult);

        RecipeAnalysisRequestEntity? capturedEntity = null;
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<RecipeAnalysisRequestEntity>()))
            .Callback<RecipeAnalysisRequestEntity>(entity => capturedEntity = entity);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedEntity.ShouldNotBeNull();
        capturedEntity.Recipes.ShouldHaveSingleItem();

        var capturedRecipe = capturedEntity.Recipes.First();
        var sourceRecipe = aiResult.First();

        capturedRecipe.ShouldMatch(sourceRecipe);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnIdOfCreatedEntity()
    {
        // Arrange
        var command = RecipeAnalysisDataFactory.CreateValidCommand();

        var expectedGuid = Guid.NewGuid();

        _aiServiceMock.Setup(s =>
                s.GetRecipeFromImagesAsync(It.IsAny<List<ImageInput>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<RecipeAnalysisRequestEntity>()))
            .Callback<RecipeAnalysisRequestEntity>(entity => { entity.Id = expectedGuid; });

        // Act
        var resultId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultId.ShouldBe(expectedGuid);
    }
}