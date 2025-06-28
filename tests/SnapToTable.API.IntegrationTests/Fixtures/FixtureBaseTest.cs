using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace SnapToTable.API.IntegrationTests.Fixtures;

public class FixtureBaseTest : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().Build();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddInMemoryCollection([
                new KeyValuePair<string, string?>("MongoDbSettings:ConnectionString", _mongoDbContainer.GetConnectionString())
            ]);
        });
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();
    }

    public new async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
        await _mongoDbContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}