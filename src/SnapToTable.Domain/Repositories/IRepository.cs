using System.Linq.Expressions;
using SnapToTable.Domain.Common;

namespace SnapToTable.Domain.Repositories;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task AddRangeAsync(IReadOnlyList<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(Guid id);
    Task<PagedResult<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        Expression<Func<T, bool>>? filter = null,
        params SortDescriptor<T>[]? sortOrder);
    Task<PagedResult<TProjection>> GetPagedAsync<TProjection>(
        int pageNumber,
        int pageSize,
        Expression<Func<T, TProjection>> projection,
        Expression<Func<T, bool>>? filter = null,
        params SortDescriptor<T>[]? sortOrder);
}