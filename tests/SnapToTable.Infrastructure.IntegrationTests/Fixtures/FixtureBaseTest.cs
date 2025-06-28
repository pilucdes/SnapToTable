using MongoDB.Driver;
using SnapToTable.Infrastructure.Data.Configuration;
using Testcontainers.MongoDb;
using Xunit;

namespace SnapToTable.Infrastructure.IntegrationTests.Fixtures;

public class FixtureBaseTest : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().Build();
    public IMongoClient MongoClient { get; private set; } = null!;
    public MongoDbSettings MongoDbSettings { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        
        MongoDbSettings = new MongoDbSettings
        {
            ConnectionString = _mongoDbContainer.GetConnectionString(),
            UseTls = false 
        };
        
        MongoClient = MongoClientConfiguration.CreateClient(MongoDbSettings);
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
    }
}