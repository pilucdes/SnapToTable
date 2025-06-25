using Mongo2Go;
using MongoDB.Driver;
using SnapToTable.Infrastructure.Data.Configuration;

namespace SnapToTable.Infrastructure.IntegrationTests;

public class BaseTest : IDisposable
{
    protected readonly IMongoDatabase Database;
    private readonly MongoDbRunner _runner;

    protected BaseTest()
    {
        _runner = MongoDbRunner.Start();
        var setting = new MongoDbSettings()
        {
            ConnectionString = _runner.ConnectionString,
            UseTls = false,
            DatabaseName = Guid.NewGuid().ToString()
        };

        IMongoClient mongoClient = MongoClientConfiguration.CreateClient(setting);
        Database = mongoClient.GetDatabase(setting.DatabaseName);
    }

    public void Dispose()
    {
        _runner.Dispose();
    }
}