using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using SnapToTable.API.IntegrationTests.Fixtures;
using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.Repositories;
using SnapToTable.Tests.Common.Builders;
using Xunit;

namespace SnapToTable.API.IntegrationTests.Controllers;

public class RecipesControllerTests : BaseApiTest
{
    private static string Url => "/api/v1/recipes";

    public RecipesControllerTests(FixtureBaseTest fixtureBase) : base(fixtureBase)
    {
    }

    [Fact]
    public async Task GetAll_Recipes_Should_Return_PagedResultOfRecipeSummaryDto()
    {
        // Arrange
        var recipesToInsert = Enumerable.Range(1, 3)
            .Select(i => new RecipeBuilder()
                .WithRecipeAnalysisId(Guid.NewGuid())
                .WithName($"Name {i}")
                .WithCategory($"Category {i}")
                .WithServings(i)
                .WithPrepTime(TimeSpan.FromMinutes(i))
                .WithCookTime(TimeSpan.FromMinutes(2))
                .WithAdditionalTime(TimeSpan.FromMinutes(3))
                .WithIngredients(["First ingredient", "Second ingredient"])
                .WithDirections(["Step 1", "Step 2"])
                .WithNotes([$"First note {i}", $"Second note {i}"])
                .Build()).ToList();
        
        await SeedDatabaseWithManyAsync(recipesToInsert,RecipeRepository.CollectionName);

        // Act
        var response = await Client.GetAsync($"{Url}");

        // Assert
        response.EnsureSuccessStatusCode();

        var pagedRecipesDto = await response.Content.ReadFromJsonAsync<PagedResultDto<RecipeSummaryDto>>();
        pagedRecipesDto.ShouldNotBeNull();
        pagedRecipesDto.Items.Count.ShouldBe(3);
        
        var recipesDto = pagedRecipesDto.Items;
        
        for (var i = 0; i < recipesDto.Count; i++)
        {
            var recipe = recipesDto[i];
            var expected = recipesToInsert[i];
        
            recipe.Id.ShouldBe(expected.Id);
            recipe.RecipeAnalysisId.ShouldBe(expected.RecipeAnalysisId);
            recipe.Name.ShouldBe(expected.Name);
            recipe.Category.ShouldBe(expected.Category);
            recipe.Ingredients.ShouldBe(expected.Ingredients);
        }
       
    }
    
    [Fact]
    public async Task GetAll_WithInvalidParam_Should_Return_Should_Return_BadRequest_WithValidationError()
    {
        // Act
        var response = await Client.GetAsync($"{Url}?page=-1");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.ShouldNotBeNull();
        result.Title.ShouldBe("One or more validation errors occurred.");
    }
    
    [Fact]
    public async Task GetById_Recipe_Should_Return_RecipeDto()
    {
        // Arrange
        var recipeToInsert = new RecipeBuilder()
            .WithRecipeAnalysisId(Guid.NewGuid())
            .WithName("Name 1")
            .WithCategory("Category 1")
            .WithServings(4)
            .WithPrepTime(TimeSpan.FromMinutes(1))
            .WithCookTime(TimeSpan.FromMinutes(2))
            .WithAdditionalTime(TimeSpan.FromMinutes(3))
            .WithIngredients(["First ingredient", "Second ingredient"])
            .WithDirections(["Step 1", "Step 2"])
            .WithNotes(["First note", "Second note"])
            .Build();

        await SeedDatabaseWithAsync(recipeToInsert, RecipeRepository.CollectionName);

        // Act
        var response = await Client.GetAsync($"{Url}/{recipeToInsert.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var recipeDto = await response.Content.ReadFromJsonAsync<RecipeDto>();
        recipeDto.ShouldNotBeNull();
        recipeDto.Id.ShouldBe(recipeToInsert.Id);
        recipeDto.Name.ShouldBe("Name 1");
        recipeDto.Category.ShouldBe("Category 1");
        recipeDto.Servings.ShouldBe(4);
        recipeDto.PrepTime.ShouldBe(TimeSpan.FromMinutes(1));
        recipeDto.CookTime.ShouldBe(TimeSpan.FromMinutes(2));
        recipeDto.AdditionalTime.ShouldBe(TimeSpan.FromMinutes(3));
        recipeDto.Ingredients.ShouldBe(["First ingredient", "Second ingredient"]);
        recipeDto.Directions.ShouldBe(["Step 1", "Step 2"]);
        recipeDto.Notes.ShouldBe(["First note", "Second note"]);
    }

    [Fact]
    public async Task GetById_WithInvalidParam_Should_Return_BadRequest_WithValidationError()
    {
        // Act
        var response = await Client.GetAsync($"{Url}/{Guid.Empty}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.ShouldNotBeNull();
        result.Title.ShouldBe("One or more validation errors occurred.");
    }
}