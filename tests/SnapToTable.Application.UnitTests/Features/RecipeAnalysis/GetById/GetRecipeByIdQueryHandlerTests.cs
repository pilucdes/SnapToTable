using Moq;
using Shouldly;
using SnapToTable.Application.Contracts;
using SnapToTable.Application.Exceptions;
using SnapToTable.Application.Features.RecipeAnalysis.GetById;
using SnapToTable.Domain.Repositories;
using SnapToTable.Tests.Common.Builders;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysis.GetById;

public class GetRecipeByIdQueryHandlerTests
{
    private readonly Mock<IRecipeAnalysisRepository> _mockRepo;
    private readonly GetRecipeByIdQueryHandler _handler;

    public GetRecipeByIdQueryHandlerTests()
    {
        _mockRepo = new Mock<IRecipeAnalysisRepository>();
        _handler = new GetRecipeByIdQueryHandler(_mockRepo.Object);
    }

    [Fact]
    public async Task Handle_WhenAnalysisIsFound_ShouldReturnCorrectRecipeAnalysisDto()
    {
        // Arrange
        var query = new GetRecipeByIdQuery(Guid.NewGuid());

        var recipeAnalysisEntity = new RecipeAnalysisBuilder()
            .WithId(query.Id)
            .WithCreatedAt(DateTime.UtcNow)
            .WithRecipe(rb => rb
                .WithName("Test Recipe 1")
                .WithCategory("Test Category")
                .WithServings(4)
                .WithIngredients(["Ingredient A", "Ingredient B"])
            )
            .Build();

        _mockRepo.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync(recipeAnalysisEntity);

        // Act
        var resultDto = await _handler.Handle(query, CancellationToken.None);

        // Assert
        resultDto.ShouldNotBeNull();
        resultDto.Id.ShouldBe(recipeAnalysisEntity.Id);
        resultDto.CreatedAt.ShouldBe(recipeAnalysisEntity.CreatedAt, tolerance: TimeSpan.FromSeconds(1));

        var recipeDto = resultDto.Recipes.ShouldHaveSingleItem();
        recipeDto.Name.ShouldBe("Test Recipe 1");
        recipeDto.Category.ShouldBe("Test Category");
        recipeDto.Servings.ShouldBe(4);
        recipeDto.Ingredients.ShouldBe(["Ingredient A", "Ingredient B"]);
    }

    [Fact]
    public async Task Handle_WhenAnalysisIsNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var query = new GetRecipeByIdQuery(Guid.NewGuid());

        _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.RecipeAnalysis)null!);

        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
        
        exception.Message.ShouldContain(nameof(RecipeAnalysis));
        exception.Message.ShouldContain(query.Id.ToString());
    }
}