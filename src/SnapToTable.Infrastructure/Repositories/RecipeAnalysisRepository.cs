using MongoDB.Driver;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;
using SnapToTable.Infrastructure.Data;

namespace SnapToTable.Infrastructure.Repositories;

public class RecipeAnalysisRepository : BaseRepository<RecipeAnalysis>, IRecipeAnalysisRepository
{
    public const string CollectionName = "recipeAnalysis";
    public RecipeAnalysisRepository(IMongoDatabase database)
        : base(database, CollectionName)
    {
    }
}