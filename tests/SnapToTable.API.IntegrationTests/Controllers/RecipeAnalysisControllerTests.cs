using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using SnapToTable.API.DTOs;
using SnapToTable.API.IntegrationTests.Fixtures;
using SnapToTable.Infrastructure.DTOs;
using Xunit;

namespace SnapToTable.API.IntegrationTests.Controllers;

public class RecipeAnalysisControllerTests : BaseApiTest
{
    public RecipeAnalysisControllerTests(FixtureBaseTest fixtureBase) : base(fixtureBase)
    {
    }

    private static string UrlRecipeAnalysis => "/api/v1/recipeanalysis";
    
    [Fact]
    public async Task Create_RecipeAnalysis_Should_Be_Created()
    {
        // Arrange
        using var multipartContent = new MultipartFormDataContent();
        
        var imageContent = CreateFakeFileContent("This is an image", "image/jpeg");
        multipartContent.Add(imageContent, nameof(CreateRecipeAnalysisRequest.Images), "test1.jpg");
        
        var rawRecipes = new RawRecipesDto()
        {
            Recipes =
            [
                new RawRecipeDto
                {
                    Category = "Dessert",
                    PrepTime = "20 minutes",
                    AdditionalTime = "5 minutes",
                    CookTime = "15 minutes",
                    Directions = ["Direction1", "Direction2"],
                    Ingredients = ["Ingredient1", "Ingredient2"],
                    Notes = ["Note1", "Note2"],
                    Name = "Recipe1",
                    Servings = "4"
                }
            ]
        };
        
        MockOpenAiProvider.SetupSuccessResponse(rawRecipes);
        
        // Act
        var response = await Client.PostAsync(UrlRecipeAnalysis, multipartContent);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        var resultGuid = await response.Content.ReadFromJsonAsync<Guid>();
        resultGuid.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task Create_WithInvalidContentType_Should_ReturnBadRequest_WithValidationError()
    {
        // Arrange
        using var multipartContent = new MultipartFormDataContent();
        
        var badImageContent = CreateFakeFileContent("This is fake image 1", "image/bad");
        multipartContent.Add(badImageContent, "Images", "test1.bad");
        
        // Act
        var response = await Client.PostAsync(UrlRecipeAnalysis, multipartContent);
        
        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        
        var result = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        result.ShouldNotBeNull();
        result.Title.ShouldBe("One or more validation errors occurred.");
    }
    
    private StreamContent CreateFakeFileContent(string content, string contentType)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        var streamContent = new StreamContent(stream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        return streamContent;
    }
}