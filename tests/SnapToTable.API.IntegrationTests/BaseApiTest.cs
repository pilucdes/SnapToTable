using MongoDB.Driver;
using SnapToTable.API.IntegrationTests.Fixtures;
using Xunit;

namespace SnapToTable.API.IntegrationTests;

public class BaseApiTest : IClassFixture<BaseApiTest>, IAsyncLifetime
{
    protected readonly HttpClient Client;
    
    private readonly IMongoClient _mongoClient;
    private readonly string _databaseName;

    public BaseApiTest(FixtureApiTest fixtureApi)
    {
        Client = fixtureApi.CreateClient();
        _mongoClient = fixtureApi.MongoClient;
        _databaseName = fixtureApi.DatabaseName;
    }
    
    public Task InitializeAsync()
    {
      return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        await _mongoClient.DropDatabaseAsync(_databaseName);
    }
}