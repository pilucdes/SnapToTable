using Moq;
using Shouldly;
using SnapToTable.Application.Features.Recipe.GetAll;
using SnapToTable.Domain.Common;
using SnapToTable.Domain.Repositories;
using SnapToTable.Tests.Common.Builders;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.Recipe.GetAll;

public class GetAllRecipesQueryHandlerTests
{
    private readonly Mock<IRecipeRepository> _repositoryMock;
    private readonly GetAllRecipesQueryHandler _handler;

    public GetAllRecipesQueryHandlerTests()
    {
        _repositoryMock = new Mock<IRecipeRepository>();
        _handler = new GetAllRecipesQueryHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_CorrectlyMapRecipeRecordToDto_WhenRecipesExist()
    {
        // --- Arrange ---
        var query = new GetAllRecipesQuery(Page: 1, PageSize: 10, Filter: "Soup", RecipeAnalysisId: null);

        var tomatoSoup = new RecipeBuilder()
            .WithName("Tomato Soup")
            .WithCategory("Vegetarian")
            .WithPrepTime(TimeSpan.FromMinutes(10))
            .WithCookTime(TimeSpan.FromMinutes(20))
            .WithAdditionalTime(null)
            .WithServings(4)
            .WithIngredients(["Tomatoes", "Onion", "Garlic"])
            .WithDirections(["Chop vegetables", "Sauté", "Simmer"])
            .WithNotes(["Serve with bread"])
            .Build();

        var chickenSoup = new RecipeBuilder()
            .WithName("Chicken Noodle Soup")
            .WithCategory("Poultry")
            .WithPrepTime(TimeSpan.FromMinutes(15))
            .WithCookTime(TimeSpan.FromMinutes(45))
            .WithAdditionalTime(TimeSpan.FromMinutes(5))
            .WithServings(6)
            .WithIngredients(["Chicken", "Noodles", "Carrots"])
            .WithDirections(["Boil chicken", "Add vegetables", "Add noodles"])
            .WithNotes([])
            .Build();

        var recipeEntities = new List<Domain.Entities.Recipe> { tomatoSoup, chickenSoup };

        var pagedResultFromRepo =
            new PagedResult<Domain.Entities.Recipe>(recipeEntities, 50, 1, 20);

        _repositoryMock
            .Setup(repo => repo.GetPagedAsync(query.Filter, query.RecipeAnalysisId, p => p, query.Page, query.PageSize))
            .ReturnsAsync(pagedResultFromRepo);

        // --- Act ---
        var result = await _handler.Handle(query, CancellationToken.None);

        // --- Assert ---
        result.ShouldNotBeNull();
        result.Items.Count.ShouldBe(2);
        result.TotalCount.ShouldBe(50);
        result.Page.ShouldBe(query.Page);
        result.PageSize.ShouldBe(query.PageSize);

        var firstDto = result.Items.First();
        var firstEntity = recipeEntities.First();

        firstDto.Id.ShouldBe(firstEntity.Id);
        firstDto.CreatedAt.ShouldBe(firstEntity.CreatedAt);

        firstDto.RecipeAnalysisId.ShouldBe(firstEntity.RecipeAnalysisId);
        firstDto.Name.ShouldBe(firstEntity.Name);
        firstDto.Category.ShouldBe(firstEntity.Category);
        firstDto.PrepTime.ShouldBe(firstEntity.PrepTime);
        firstDto.Servings.ShouldBe(firstEntity.Servings);
        firstDto.Ingredients.ShouldBe(firstEntity.Ingredients);
        firstDto.Directions.ShouldBe(firstEntity.Directions);
        firstDto.Notes.ShouldBe(firstEntity.Notes);

        _repositoryMock.Verify(
            repo => repo.GetPagedAsync(query.Filter, query.RecipeAnalysisId, p => p, query.Page, query.PageSize),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WithValidQuery_ShouldCallDependencies()
    {
        // Arrange
        var query = new GetAllRecipesQuery(Page: 1, PageSize: 10, Filter: "Soup", RecipeAnalysisId: null);

        _repositoryMock
            .Setup(repo => repo.GetPagedAsync(query.Filter, query.RecipeAnalysisId, p => p, query.Page, query.PageSize))
            .ReturnsAsync(new PagedResult<Domain.Entities.Recipe>(new List<Domain.Entities.Recipe>(), 1, 1, 20));

        // Act
        await _handler.Handle(query, CancellationToken.None);
        
        // Assert
        _repositoryMock.Verify(
            repo => repo.GetPagedAsync(query.Filter, query.RecipeAnalysisId, p => p, query.Page, query.PageSize),
            Times.Once);
    }
}