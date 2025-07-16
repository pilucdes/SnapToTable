using MongoDB.Driver;
using SnapToTable.Infrastructure.IntegrationTests.Constants;
using SnapToTable.Infrastructure.IntegrationTests.Fixtures;
using Xunit;

namespace SnapToTable.Infrastructure.IntegrationTests;

[CollectionDefinition(TestCollectionNames.MongoDbCollection)]
public class MongoDbCollection : ICollectionFixture<FixtureBaseTest>;

[Collection(TestCollectionNames.MongoDbCollection)]
public abstract class BaseTest : IAsyncLifetime
{
    private readonly IMongoClient _mongoClient;
    protected readonly IMongoDatabase Database;
    private readonly string _databaseName;

    protected BaseTest(FixtureBaseTest fixture)
    {
        _mongoClient = fixture.MongoClient;
        _databaseName = $"test_db_{Guid.NewGuid()}";
        Database = _mongoClient.GetDatabase(_databaseName);
    }
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _mongoClient.DropDatabaseAsync(_databaseName);
    }
    
    protected async Task SeedDatabaseWithAsync<T>(T entity,string collectionName) where T : class
    {
        await Database.GetCollection<T>(collectionName).InsertOneAsync(entity);
    }
    
    protected async Task SeedDatabaseWithManyAsync<T>(IReadOnlyList<T> entities,string collectionName) where T : class
    {
        if (!entities.Any())
            return;
        
        await Database.GetCollection<T>(collectionName).InsertManyAsync(entities);
    }
}