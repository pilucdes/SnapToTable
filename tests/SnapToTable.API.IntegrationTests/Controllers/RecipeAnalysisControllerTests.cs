using MongoDB.Driver;
using Shouldly;
using SnapToTable.API.IntegrationTests.Fixtures;
using Xunit;

namespace SnapToTable.API.IntegrationTests.Controllers;

public class RecipeAnalysisControllerTests : BaseApiTest
{
    public RecipeAnalysisControllerTests(FixtureApiTest fixtureApi) : base(fixtureApi)
    {
    }

    [Fact]
    public async Task Create_AnalysisRecipes_Should_Be_Created()
    {
        //Act
        var response = await Client.PostAsync("/api/v1/recipeanalysis", null);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadAsStringAsync();
        
        // Assert
        result.ShouldNotBeNullOrEmpty();
        Guid.TryParse(result, out _).ShouldBeTrue();
    }
    
}