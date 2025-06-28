using MongoDB.Driver;
using SnapToTable.Infrastructure.IntegrationTests.Constants;
using SnapToTable.Infrastructure.IntegrationTests.Fixtures;
using Xunit;

[CollectionDefinition(TestCollectionNames.MongoDbCollection)]
public class MongoDbCollection : ICollectionFixture<FixtureBaseTest>;

[Collection(TestCollectionNames.MongoDbCollection)]
public abstract class BaseTest : IAsyncLifetime
{
    private readonly IMongoClient _mongoClient;
    protected readonly IMongoDatabase Database;
    protected readonly string DatabaseName;

    protected BaseTest(FixtureBaseTest fixture)
    {
        _mongoClient = fixture.MongoClient;
        DatabaseName = $"test_db_{Guid.NewGuid()}";
        Database = _mongoClient.GetDatabase(DatabaseName);
    }
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _mongoClient.DropDatabaseAsync(DatabaseName);
    }
}