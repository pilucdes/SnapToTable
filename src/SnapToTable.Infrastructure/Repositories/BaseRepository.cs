using System.Linq.Expressions;
using MongoDB.Driver;
using SnapToTable.Domain.Common;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;
using SortDirection = SnapToTable.Domain.Common.SortDirection;

namespace SnapToTable.Infrastructure.Repositories;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public BaseRepository(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        await _collection.InsertOneAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var id = entity.Id;
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }

    public async Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize,
        Expression<Func<T, bool>>? filter = null,
        params SortDescriptor<T>[]? sortOrder)
    {
        return await GetPagedAsync(pageNumber, pageSize, p => p, filter, sortOrder);
    }

    public async Task<PagedResult<TProjection>> GetPagedAsync<TProjection>(int pageNumber, int pageSize,
        Expression<Func<T, TProjection>> projection, Expression<Func<T, bool>>? filter = null,
        params SortDescriptor<T>[]? sortOrder)
    {
        var finalFilter = filter ?? Builders<T>.Filter.Empty;
        var totalCount = await _collection.CountDocumentsAsync(finalFilter);
        var findFluent = _collection.Find(finalFilter);
        
        var finalSort = GetSortDefinition(sortOrder);

        var items = await findFluent
            .Sort(finalSort)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .Project(projection)
            .ToListAsync();

        return new PagedResult<TProjection>(items, totalCount, pageNumber, pageSize);
    }

    private static SortDefinition<T> GetSortDefinition(SortDescriptor<T>[]? sortOrder)
    {
        SortDefinition<T> finalSort;
        if (sortOrder is not null && sortOrder.Length > 0)
        {
            var sortBuilder = Builders<T>.Sort;
            var sortDefinitions = sortOrder.Select(sd => sd.Direction == SortDirection.Ascending
                ? sortBuilder.Ascending(sd.KeySelector)
                : sortBuilder.Descending(sd.KeySelector));

            finalSort = sortBuilder.Combine(sortDefinitions);
        }
        else
        {
            finalSort = Builders<T>.Sort.Descending(e => e.Id);
        }

        return finalSort;
    }
}