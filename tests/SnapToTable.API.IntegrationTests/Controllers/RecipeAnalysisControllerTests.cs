using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using SnapToTable.API.DTOs;
using SnapToTable.API.IntegrationTests.Builders;
using SnapToTable.API.IntegrationTests.Factories;
using SnapToTable.API.IntegrationTests.Fixtures;
using SnapToTable.Infrastructure.DTOs;
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
    public async Task Create_WithInvalidContentType_Should_ReturnBadRequest_WithValidationError()
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
    
}