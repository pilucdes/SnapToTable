using MongoDB.Driver;
using SnapToTable.Infrastructure.Data.Configuration;
using Testcontainers.MongoDb;
using Xunit;

namespace SnapToTable.Infrastructure.IntegrationTests;

public class BaseTest : IAsyncLifetime
{
    protected IMongoDatabase Database = null!;
    private readonly MongoDbContainer _mongoDbContainer;

    protected BaseTest()
    {
        _mongoDbContainer = new MongoDbBuilder().Build();
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
        var setting = new MongoDbSettings()
        {
            ConnectionString = _mongoDbContainer.GetConnectionString(),
            UseTls = false,
            DatabaseName = Guid.NewGuid().ToString()
        };

        IMongoClient mongoClient = MongoClientConfiguration.CreateClient(setting);
        Database = mongoClient.GetDatabase(setting.DatabaseName);
    }

    public async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
        await _mongoDbContainer.DisposeAsync();
    }
}