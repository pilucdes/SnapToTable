using System.Linq.Expressions;
using MongoDB.Driver;
using SnapToTable.Domain.Entities;
using SnapToTable.Domain.Repositories;

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

    public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.Id = Guid.CreateVersion7();
        await _collection.InsertOneAsync(entity);
    }

    public virtual async Task UpdateAsync(T entity)
    {
        var id = GetIdValue(entity);
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.ReplaceOneAsync(filter, entity);
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        await _collection.DeleteOneAsync(filter);
    }

    public virtual async Task<bool> ExistsAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq("_id", id);
        return await _collection.Find(filter).AnyAsync();
    }

    private Guid GetIdValue(T entity)
    {
        var property = typeof(T).GetProperty("Id");
        if (property == null)
            throw new InvalidOperationException("Entity must have an Id property");

        var value = property.GetValue(entity);
        if (value == null)
            throw new InvalidOperationException("Entity Id cannot be null");
        
        if (value is Guid guidValue)
            return guidValue;
        
        if (Guid.TryParse(value.ToString(), out Guid parsedGuid))
            return parsedGuid;
        
        throw new InvalidOperationException($"Cannot convert Id value '{value}' to Guid");
    }
}