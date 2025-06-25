using Moq;
using Shouldly;
using SnapToTable.Application.Contracts;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;
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
    public async Task Handle_Should_CallAiService_MapResultToEntity_And_AddToRepository()
    {
        var command = new CreateRecipeAnalysisRequestCommand(new List<ImageInput>
        {
            new(new MemoryStream([1, 2, 3]), "image/jpeg")
        });
        
        var aiExtractionResult = CreateRecipeExtractionResults();
        
        _aiServiceMock
            .Setup(s => s.GetRecipeFromImagesAsync(command.Images, It.IsAny<CancellationToken>()))
            .ReturnsAsync(aiExtractionResult);
        
        RecipeAnalysisRequestEntity? capturedAnalysisRequest = null;
        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<RecipeAnalysisRequestEntity>()))
            .Callback<RecipeAnalysisRequestEntity>(entity => capturedAnalysisRequest = entity)
            .Returns(Task.CompletedTask);
        
        // Act
        var resultId = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _aiServiceMock.Verify(
            s => s.GetRecipeFromImagesAsync(command.Images, It.IsAny<CancellationToken>()),
            Times.Once);
        
        _repositoryMock.Verify(
            r => r.AddAsync(It.IsAny<RecipeAnalysisRequestEntity>()),
            Times.Once);
        
        capturedAnalysisRequest.ShouldNotBeNull();
        capturedAnalysisRequest.Recipes.ShouldHaveSingleItem();

        var capturedRecipe = capturedAnalysisRequest.Recipes.First();
        var sourceRecipe = aiExtractionResult.First();

        capturedRecipe.Name.ShouldBe(sourceRecipe.Name);
        capturedRecipe.Category.ShouldBe(sourceRecipe.Category);
        capturedRecipe.PrepTime.ShouldBe(sourceRecipe.PrepTime);
        capturedRecipe.CookTime.ShouldBe(sourceRecipe.CookTime);
        capturedRecipe.AdditionalTime.ShouldBe(sourceRecipe.AdditionalTime);
        capturedRecipe.Servings.ShouldBe(sourceRecipe.Servings);
        capturedRecipe.Ingredients.ShouldBe(sourceRecipe.Ingredients);
        capturedRecipe.Directions.ShouldBe(sourceRecipe.Directions);
        capturedRecipe.Notes.ShouldBe(sourceRecipe.Notes);
        
        resultId.ShouldBe(capturedAnalysisRequest.Id);
    }

    private static List<RecipeExtractionResult> CreateRecipeExtractionResults()
    {
        var aiExtractionResult = new List<RecipeExtractionResult>
        {
            new()
            {
                Name = "Test Chocolate Cake",
                Category = "Dessert",
                PrepTime = TimeSpan.FromMinutes(15),
                CookTime = TimeSpan.FromMinutes(30),
                AdditionalTime = TimeSpan.FromMinutes(10),
                Servings = 8,
                Ingredients = ["1 cup flour", "1 cup sugar"],
                Directions = ["Mix ingredients", "Bake at 350F"],
                Notes = ["Enjoy!"]
            }
        };
        return aiExtractionResult;
    }
}