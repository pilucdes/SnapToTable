using MongoDB.Driver;
using Shouldly;
using SnapToTable.Domain.Entities;
using SnapToTable.Infrastructure.Data.Configuration;
using SnapToTable.Infrastructure.IntegrationTests.Fixtures;
using SnapToTable.Infrastructure.Repositories;
using Xunit;

namespace SnapToTable.Infrastructure.IntegrationTests.Services;

public class MongoDbIndexInitializerIntegrationTests : BaseTest
{
    private readonly IMongoCollection<Recipe> _recipeCollection;

    public MongoDbIndexInitializerIntegrationTests(FixtureBaseTest fixture) : base(fixture)
    {
        _recipeCollection = Database.GetCollection<Recipe>(RecipeRepository.CollectionName);
    }

    [Fact]
    public async Task CreateIndexesAsync_ShouldCreateCorrectIndexesOnRecipeCollection()
    {
        // Arrange
        var initializer = new MongoDbIndexInitializer(Database);

        // Act
        await initializer.CreateIndexesAsync();

        // Assert
        var createdIndexes = await (await _recipeCollection.Indexes.ListAsync()).ToListAsync();
        createdIndexes.Count.ShouldBe(3);
        
        var analysisIdIndex = createdIndexes.Single(idx => idx["name"] == "Recipe_RecipeAnalysisId_IX");
        analysisIdIndex["key"]["RecipeAnalysisId"].AsInt32.ShouldBe(1);
        
        var textSearchIndex = createdIndexes.Single(idx => idx["name"] == "Recipe_TextSearch_IX");
        textSearchIndex["key"]["_fts"].AsString.ShouldBe("text");
        
        var weightsDoc = textSearchIndex["weights"].AsBsonDocument;
        weightsDoc.ShouldNotBeNull();

        weightsDoc.Names.OrderBy(n => n).ShouldBe(new[] { "Category", "Ingredients", "Name" }.OrderBy(n => n));
    }
}