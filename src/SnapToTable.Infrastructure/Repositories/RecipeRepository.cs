using System.Linq.Expressions;
using MongoDB.Driver;
using SnapToTable.Domain.Common;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;

namespace SnapToTable.Infrastructure.Repositories;

public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
{
    public const string CollectionName = "recipes";

    public RecipeRepository(IMongoDatabase database)
        : base(database, CollectionName)
    {
    }

    public async Task<PagedResult<TProjection>> GetPagedAsync<TProjection>(string? filter,
        Guid? recipeAnalysisId,
        Expression<Func<Recipe, TProjection>> projection,
        int pageNumber,
        int pageSize)
    {
        var filterBuilder = Builders<Recipe>.Filter;
        var combinedFilter = filterBuilder.Empty;
        
        if (recipeAnalysisId.HasValue)
        {
            combinedFilter &= filterBuilder.Eq(nameof(Recipe.RecipeAnalysisId), recipeAnalysisId.Value);
        }

        if (!string.IsNullOrEmpty(filter))
        {
            combinedFilter &= filterBuilder.Text(filter);
        }
        
        var totalCount = await _collection.CountDocumentsAsync(combinedFilter);

        var items = await _collection.Find(combinedFilter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize).Project(projection)
            .ToListAsync();

        return new PagedResult<TProjection>(items, totalCount, pageNumber, pageSize);
    }
}