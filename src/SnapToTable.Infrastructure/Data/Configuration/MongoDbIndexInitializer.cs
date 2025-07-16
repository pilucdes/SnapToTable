using MongoDB.Driver;
using SnapToTable.Domain.Entities;
using SnapToTable.Infrastructure.Repositories;

namespace SnapToTable.Infrastructure.Data.Configuration;

public interface IMongoDbIndexInitializer
{
    Task CreateIndexesAsync();
}

public class MongoDbIndexInitializer : IMongoDbIndexInitializer
{
    private readonly IMongoDatabase _database;

    public MongoDbIndexInitializer(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task CreateIndexesAsync()
    {
        var recipeCollection = _database.GetCollection<Recipe>(RecipeRepository.CollectionName);
        
        var recipeAnalysisIdIndexModel = new CreateIndexModel<Recipe>(
            Builders<Recipe>.IndexKeys.Ascending(r => r.RecipeAnalysisId),
            new CreateIndexOptions 
            { 
                Name = "Recipe_RecipeAnalysisId_IX"
            }
        );
        await recipeCollection.Indexes.CreateOneAsync(recipeAnalysisIdIndexModel);
        
        var textSearchIndexModel = new CreateIndexModel<Recipe>(
            Builders<Recipe>.IndexKeys.Combine(
                Builders<Recipe>.IndexKeys.Text(r => r.Name),
                Builders<Recipe>.IndexKeys.Text(r => r.Category),
                Builders<Recipe>.IndexKeys.Text(r => r.Ingredients)
            ),
            new CreateIndexOptions { Name = "Recipe_TextSearch_IX" }
        );
        await recipeCollection.Indexes.CreateOneAsync(textSearchIndexModel);

    }
}