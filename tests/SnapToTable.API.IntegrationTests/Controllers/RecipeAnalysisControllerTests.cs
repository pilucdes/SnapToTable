using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using SnapToTable.API.IntegrationTests.Builders;
using SnapToTable.API.IntegrationTests.Fixtures;
using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.Repositories;
using SnapToTable.Tests.Common.Builders;
using Xunit;

namespace SnapToTable.API.IntegrationTests.Controllers;

public class RecipeAnalysisControllerTests : BaseApiTest
{
    private static string Url => "/api/v1/recipeanalysis";

    public RecipeAnalysisControllerTests(FixtureBaseTest fixtureBase) : base(fixtureBase)
    {
    }

    [Fact]
    public async Task Create_RecipeAnalysis_Should_Be_Created()
    {
        // Arrange
        var multipartContent = new MultipartFormDataContentBuilder()
            .WithValidImage()
            .Build();

        var expectedRecipes = new RawRecipesDtoBuilder().Build();

        MockOpenAiProvider.SetupSuccessResponse(expectedRecipes);

        // Act
        var response = await Client.PostAsync(Url, multipartContent);

        // Assert
        response.EnsureSuccessStatusCode();

        var resultGuid = await response.Content.ReadFromJsonAsync<Guid>();
        resultGuid.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Create_WithInvalidContentType_Should_Return_BadRequest_WithValidationError()
    {
        // Arrange
        var multipartContent = new MultipartFormDataContentBuilder()
            .WithInvalidImageContentType()
            .Build();

        // Act
        var response = await Client.PostAsync(Url, multipartContent);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.ShouldNotBeNull();
        result.Title.ShouldBe("One or more validation errors occurred.");
    }

    [Fact]
    public async Task GetById_RecipeAnalysis_Should_Return_RecipeAnalysisDto()
    {
        // Arrange
        var recipeAnalysisToInsert = new RecipeAnalysisBuilder()
            .WithRecipe(rb => rb
                .WithName("Name 1")
                .WithCategory("Category 1")
                .WithServings(4)
                .WithPrepTime(TimeSpan.FromMinutes(1))
                .WithCookTime(TimeSpan.FromMinutes(2))
                .WithAdditionalTime(TimeSpan.FromMinutes(3))
                .WithIngredients(["First ingredient", "Second ingredient"])
                .WithDirections(["Step 1", "Step 2"])
                .WithNotes(["First note", "Second note"])
            )
            .Build();

        await SeedDatabaseWithAsync(recipeAnalysisToInsert, RecipeAnalysisRepository.CollectionName);

        // Act
        var response = await Client.GetAsync($"{Url}/{recipeAnalysisToInsert.Id}");

        // Assert
        response.EnsureSuccessStatusCode();

        var resultDto = await response.Content.ReadFromJsonAsync<RecipeAnalysisDto>();
        resultDto.ShouldNotBeNull();
        resultDto.Id.ShouldBe(recipeAnalysisToInsert.Id);
        
        var recipeDto = resultDto.Recipes.ShouldHaveSingleItem();
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
    public async Task GetById_WithInvalidGuid_Should_Return_BadRequest_WithValidationError()
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