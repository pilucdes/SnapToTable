using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SnapToTable.Infrastructure.Data.Configuration;

namespace SnapToTable.Infrastructure.Services;

public class MongoDbInitializerHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MongoDbInitializerHostedService> _logger;

    public MongoDbInitializerHostedService(IServiceProvider serviceProvider,ILogger<MongoDbInitializerHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MongoDbInitializerHostedService is starting.");
        
        try
        {
            using var scope = _serviceProvider.CreateScope();
        
            var dbIndexInitializer = scope.ServiceProvider.GetRequiredService<IMongoDbIndexInitializer>();
            await dbIndexInitializer.CreateIndexesAsync();
            
            _logger.LogInformation("MongoDB index initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "A fatal error occurred during MongoDB index initialization.");
            throw;
        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}