using System.Text.Json;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Moq;
using SnapToTable.API.IntegrationTests.Constants;
using SnapToTable.API.IntegrationTests.Fixtures;
using SnapToTable.API.IntegrationTests.Mocks;
using SnapToTable.Infrastructure.DTOs;
using Xunit;

namespace SnapToTable.API.IntegrationTests;

[CollectionDefinition(TestCollectionNames.ApiCollection)]
public class MongoDbCollection : ICollectionFixture<FixtureBaseTest>;

[Collection(TestCollectionNames.ApiCollection)]
public abstract class BaseApiTest : IAsyncLifetime
{
    protected readonly HttpClient Client;
    protected readonly OpenAiServiceMockProvider MockOpenAiProvider;
    
    private readonly IMongoClient _mongoClient;
    private readonly string _databaseName;  
    

    public BaseApiTest(FixtureBaseTest fixtureBase)
    {
        _databaseName = $"test_db_{Guid.NewGuid()}";
        MockOpenAiProvider = new OpenAiServiceMockProvider();
        
        var appFactory = fixtureBase.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((_, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["MongoDbSettings:DatabaseName"] = _databaseName
                });
            });

            builder.ConfigureServices((_, services) =>
            {
                services.AddSingleton(MockOpenAiProvider.MockOpenAiService.Object);
            });
        });

        _mongoClient = appFactory.Services.GetRequiredService<IMongoClient>();
        Client = appFactory.CreateClient();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _mongoClient.DropDatabaseAsync(_databaseName);
    }
}