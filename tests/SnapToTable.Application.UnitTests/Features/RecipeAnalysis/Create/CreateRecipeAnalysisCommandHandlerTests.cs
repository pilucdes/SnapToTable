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
    private readonly Mock<IRecipeAnalysisRepository> _repositoryRecipeAnalysisMock;
    private readonly Mock<IRecipeRepository> _repositoryRecipeMock;
    private readonly Mock<IAiRecipeExtractionService> _aiServiceMock;
    private readonly CreateRecipeAnalysisCommandHandler _handler;

    public CreateRecipeAnalysisCommandHandlerTests()
    {
        _repositoryRecipeAnalysisMock = new Mock<IRecipeAnalysisRepository>();
        _repositoryRecipeMock = new Mock<IRecipeRepository>();
        _aiServiceMock = new Mock<IAiRecipeExtractionService>();

        _handler = new CreateRecipeAnalysisCommandHandler(_repositoryRecipeAnalysisMock.Object,
            _repositoryRecipeMock.Object, _aiServiceMock.Object);
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
        
        _repositoryRecipeAnalysisMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.RecipeAnalysis>()), Times.Once);
        _repositoryRecipeMock.Verify(r => r.AddRangeAsync(It.IsAny<IReadOnlyList<Domain.Entities.Recipe>>()), Times.Once);
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

        Domain.Entities.RecipeAnalysis? capturedRecipeAnalysis = null;
        IReadOnlyList<Domain.Entities.Recipe>? capturedRecipes = null;

        _repositoryRecipeAnalysisMock
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.RecipeAnalysis>()))
            .Callback<Domain.Entities.RecipeAnalysis>(entity => capturedRecipeAnalysis = entity);

        _repositoryRecipeMock.Setup(r => r.AddRangeAsync(It.IsAny<IReadOnlyList<Domain.Entities.Recipe>>()))
            .Callback<IReadOnlyList<Domain.Entities.Recipe>>(entities => capturedRecipes = entities);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedRecipeAnalysis.ShouldNotBeNull();
        capturedRecipes.ShouldNotBeNull();
        
        var capturedRecipe = capturedRecipes.First();
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

        _repositoryRecipeAnalysisMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.RecipeAnalysis>()))
            .Callback<Domain.Entities.RecipeAnalysis>(entity => { entity.Id = expectedGuid; });

        // Act
        var resultId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        resultId.ShouldBe(expectedGuid);
    }
}