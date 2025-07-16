using System.Linq.Expressions;
using Shouldly;
using SnapToTable.Domain.Entities;
using SnapToTable.Infrastructure.Data.Configuration;
using SnapToTable.Infrastructure.IntegrationTests.Fixtures;
using SnapToTable.Infrastructure.IntegrationTests.Models;
using SnapToTable.Infrastructure.Repositories;
using SnapToTable.Tests.Common.Builders;
using Xunit;

namespace SnapToTable.Infrastructure.IntegrationTests.Repositories;

public class RecipeRepositoryTests : BaseTest
{
    private readonly RecipeRepository _repository;
    
    public RecipeRepositoryTests(FixtureBaseTest fixture) : base(fixture)
    {
        _repository = new RecipeRepository(Database);
    }
    
    [Fact]
    public async Task GetPagedAsync_WithRecipeAnalysisIdFilter_ShouldReturnOnlyMatchingRecipes()
    {
        // Arrange
        var seededRecipes = await SeedRecipesAsync(
            new RecipeBuilder().Build(),
            new RecipeBuilder().Build(),
            new RecipeBuilder().Build()
        );
        var targetRecipe = seededRecipes[1];

        // Act
        var result = await _repository.GetPagedAsync(
            filter: null,
            recipeAnalysisId: targetRecipe.RecipeAnalysisId,
            projection: r => r,
            pageNumber: 1,
            pageSize: 5
        );

        // Assert
        result.ShouldNotBeNull();
        result.TotalCount.ShouldBe(1);
        result.Items.Count.ShouldBe(1);
        result.Items.First().Id.ShouldBe(targetRecipe.Id);
        result.Items.First().RecipeAnalysisId.ShouldBe(targetRecipe.RecipeAnalysisId);
    }

    [Fact]
    public async Task GetPagedAsync_WithTextFilter_ShouldReturnMatchingRecipes()
    {
        // Arrange
        var initializer = new MongoDbIndexInitializer(Database);
        await initializer.CreateIndexesAsync();
        
        await SeedRecipesAsync(
            new RecipeBuilder().WithName("Creamy Tomato Soup").Build(),
            new RecipeBuilder().WithName("Spicy Chicken Noodle Soup").Build(),
            new RecipeBuilder().WithName("Hearty Beef Stew").Build()
        );

        // Act
        var result = await _repository.GetPagedAsync(
            filter: "Soup",
            recipeAnalysisId: null,
            projection: r => r,
            pageNumber: 1,
            pageSize: 5
        );

        // Assert
        result.TotalCount.ShouldBe(2);
        result.Items.Count.ShouldBe(2);
        result.Items.ShouldAllBe(r => r.Name.Contains("Soup"));
        result.Items.ShouldNotContain(r => r.Name.Contains("Stew"));
    }

    [Fact]
    public async Task GetPagedAsync_WithCombinedFilters_ShouldReturnCorrectlyFilteredRecipes()
    {
        // Arrange
        var initializer = new MongoDbIndexInitializer(Database);
        await initializer.CreateIndexesAsync();
        
        var commonAnalysisId = Guid.NewGuid();
        await SeedRecipesAsync(
            new RecipeBuilder().WithName("Special Soup").WithRecipeAnalysisId(commonAnalysisId).Build(),
            new RecipeBuilder().WithName("Special Beef Stew").WithRecipeAnalysisId(commonAnalysisId).Build(),
            new RecipeBuilder().WithName("Generic Tomato Soup").Build()
        );

        // Act
        var result = await _repository.GetPagedAsync(
            filter: "\"Special Soup\"",
            recipeAnalysisId: commonAnalysisId,
            projection: r => r,
            pageNumber: 1,
            pageSize: 5
        );

        // Assert
        result.TotalCount.ShouldBe(1);
        result.Items.Count.ShouldBe(1);
        var foundRecipe = result.Items.First();
        foundRecipe.Name.ShouldBe("Special Soup");
        foundRecipe.RecipeAnalysisId.ShouldBe(commonAnalysisId);
    }

    [Fact]
    public async Task GetPagedAsync_WithPagination_ShouldReturnCorrectPageAndItemCount()
    {
        // Arrange
        await SeedRecipesAsync(7);
        int pageNumber = 2;
        int pageSize = 3;

        // Act
        var result = await _repository.GetPagedAsync(null, null, r => r, pageNumber, pageSize);

        // Assert
        result.TotalCount.ShouldBe(7);
        result.Page.ShouldBe(pageNumber);
        result.PageSize.ShouldBe(pageSize);
        result.Items.Count.ShouldBe(pageSize);
        result.TotalPages.ShouldBe(3);
    }

    [Fact]
    public async Task GetPagedAsync_WithProjection_ShouldReturnMappedObject()
    {
        // Arrange
        var seededRecipe = (await SeedRecipesAsync(1)).First();
        
        Expression<Func<Recipe, RecipeProjectedTest>> projection = r => new RecipeProjectedTest(r.Id, r.Name);
        
        // Act
        var result = await _repository.GetPagedAsync(null, null, projection, 1, 5);

        // Assert
        result.TotalCount.ShouldBe(1);
        result.Items.Count.ShouldBe(1);
        
        var firstProjectedItem = result.Items.First();
        firstProjectedItem.ShouldBeOfType<RecipeProjectedTest>();
        firstProjectedItem.Id.ShouldBe(seededRecipe.Id);
        firstProjectedItem.Name.ShouldBe(seededRecipe.Name);
    }
    
    [Fact]
    public async Task GetPagedAsync_WhenNoRecipesMatchFilters_ShouldReturnEmptyResult()
    {
        // Arrange
        var initializer = new MongoDbIndexInitializer(Database);
        await initializer.CreateIndexesAsync();
        
        await SeedRecipesAsync(5);
    
        // Act
        var result = await _repository.GetPagedAsync("NonExistentFilterString", Guid.NewGuid(), r => r, 1, 10);
    
        // Assert
        result.TotalCount.ShouldBe(0);
        result.Items.ShouldBeEmpty();
        result.Page.ShouldBe(1);
        result.TotalPages.ShouldBe(0);
    }
    
    private async Task<List<Recipe>> SeedRecipesAsync(int count)
    {
        var recipes = Enumerable.Range(1, count)
            .Select(i => new RecipeBuilder().WithName($"Recipe {i}").Build())
            .ToList();

        await SeedDatabaseWithManyAsync(recipes, RecipeRepository.CollectionName);
        return recipes;
    }
    
    private async Task<List<Recipe>> SeedRecipesAsync(params Recipe[] recipes)
    {
        await SeedDatabaseWithManyAsync(recipes, RecipeRepository.CollectionName);
        return recipes.ToList();
    }
}