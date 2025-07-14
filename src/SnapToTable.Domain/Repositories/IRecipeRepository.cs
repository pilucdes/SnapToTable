using System.Linq.Expressions;
using SnapToTable.Domain.Common;
using SnapToTable.Domain.Entities;

namespace SnapToTable.Domain.Repositories;

public interface IRecipeRepository : IRepository<Recipe>
{
    public Task<PagedResult<TProjection>> GetPagedAsync<TProjection>(string? filter,
        Guid? recipeAnalysisId,
        Expression<Func<Recipe, TProjection>> projection,
        int pageNumber,
        int pageSize);
}