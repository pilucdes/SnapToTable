using MongoDB.Driver;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Infrastructure.Repositories;

public class RecipeAnalysisRepository : BaseRepository<RecipeAnalysis>, IRecipeAnalysisRepository
{
    public const string CollectionName = "recipeAnalysis";
    public RecipeAnalysisRepository(IMongoDatabase database)
        : base(database, CollectionName)
    {
    }
}