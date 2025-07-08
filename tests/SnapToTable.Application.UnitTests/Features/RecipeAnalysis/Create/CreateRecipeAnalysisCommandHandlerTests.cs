using Moq;
using Shouldly;
using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysis.Create;
using SnapToTable.Domain.Repositories;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysis.Create;

public class CreateRecipeAnalysisCommandHandlerTests
{
    private readonly Mock<IRecipeAnalysisRepository> _repositoryMock;
    private readonly Mock<IAiRecipeExtractionService> _aiServiceMock;
    private readonly CreateRecipeAnalysisCommandHandler _handler;

    public CreateRecipeAnalysisCommandHandlerTests()
    {
        _repositoryMock = new Mock<IRecipeAnalysisRepository>();
        _aiServiceMock = new Mock<IAiRecipeExtractionService>();
        _handler = new CreateRecipeAnalysisCommandHandler(_repositoryMock.Object, _aiServiceMock.Object);
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
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.RecipeAnalysis>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCorrectlyMapAiResultToEntity()
    {
        // Arrange
        var command = RecipeAnalysisDataFactory.CreateValidCommand();
        var aiResult = RecipeAnalysisDataFactory.CreateRecipeExtractionResults();

        _aiServiceMock
            .Setup(s => s.GetRecipeFromImagesAsync(It.IsAny<List<ImageInputDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(aiResult);

        Domain.Entities.RecipeAnalysis? capturedEntity = null;
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.RecipeAnalysis>()))
            .Callback<Domain.Entities.RecipeAnalysis>(entity => capturedEntity = entity);

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
                s.GetRecipeFromImagesAsync(It.IsAny<List<ImageInputDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.RecipeAnalysis>()))
            .Callback<Domain.Entities.RecipeAnalysis>(entity => { entity.Id = expectedGuid; });

        // Act
        var resultId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultId.ShouldBe(expectedGuid);
    }
}