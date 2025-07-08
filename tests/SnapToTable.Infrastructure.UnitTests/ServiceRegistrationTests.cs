using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shouldly;
using SnapToTable.Application.Contracts;
using SnapToTable.Domain.Repositories;
using SnapToTable.Infrastructure.Data.Configuration;
using SnapToTable.Infrastructure.Repositories;
using SnapToTable.Infrastructure.Services;
using Xunit;

namespace SnapToTable.Infrastructure.UnitTests;

public class ServiceRegistrationTests
{
    [Fact]
    public void AddInfrastructure_ShouldRegisterAllServicesCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenAI:ApiKey", "fake-openai-key-for-test" },
                { "MongoDbSettings:ConnectionString", "mongodb://localhost:27017" },
                { "MongoDbSettings:DatabaseName", "TestDb" }
            })
            .Build();
        
        services.AddSingleton<IConfiguration>(configuration);

        // Act
        var resultServices = services.AddInfrastructure(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        // 1. Ensure the method is chainable
        resultServices.ShouldBeSameAs(services);
        
        var mongoOptions = serviceProvider.GetService<IOptions<MongoDbSettings>>();
        mongoOptions.ShouldNotBeNull();
        mongoOptions.Value.ConnectionString.ShouldBe("mongodb://localhost:27017");
        mongoOptions.Value.DatabaseName.ShouldBe("TestDb");
        
        AssertServiceIsRegistered<IMongoClient>(services, ServiceLifetime.Singleton);
        AssertServiceIsRegistered<IMongoDatabase>(services, ServiceLifetime.Singleton);
        AssertServiceIsRegistered<IRecipeAnalysisRepository, RecipeAnalysisRepository>(services,
            ServiceLifetime.Scoped);
        AssertServiceIsRegistered<IAiRecipeExtractionService, AiRecipeExtractionService>(services,
            ServiceLifetime.Scoped);
        
        serviceProvider.GetService<IMongoClient>().ShouldNotBeNull();
        serviceProvider.GetService<IMongoDatabase>().ShouldNotBeNull();

        using var scope = serviceProvider.CreateScope();
        scope.ServiceProvider.GetService<IRecipeAnalysisRepository>().ShouldNotBeNull();
        scope.ServiceProvider.GetService<IAiRecipeExtractionService>().ShouldNotBeNull();
    }

    [Fact]
    public void AddInfrastructure_ShouldThrow_WhenMongoDbSettingsAreMissing()
    {
        // Arrange
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "OpenAI:ApiKey", "fake-openai-key-for-test" }
            })
            .Build();

        services.AddSingleton<IConfiguration>(configuration);

        // Act
        services.AddInfrastructure(configuration);
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var exception = Should.Throw<Exception>(() => serviceProvider.GetRequiredService<IMongoDatabase>());
        
        exception.ShouldNotBeNull();
    }
    
    private void AssertServiceIsRegistered<TService>(IServiceCollection services, ServiceLifetime lifetime)
    {
        var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
        descriptor.ShouldNotBeNull($"Service of type {typeof(TService).Name} was not registered.");
        descriptor.Lifetime.ShouldBe(lifetime, $"Service {typeof(TService).Name} has incorrect lifetime.");
    }

    private void AssertServiceIsRegistered<TService, TImplementation>(IServiceCollection services,
        ServiceLifetime lifetime)
    {
        var descriptor = services.FirstOrDefault(s => s.ServiceType == typeof(TService));
        descriptor.ShouldNotBeNull($"Service of type {typeof(TService).Name} was not registered.");
        descriptor.ImplementationType.ShouldBe(typeof(TImplementation),
            $"Service {typeof(TService).Name} has incorrect implementation type.");
        descriptor.Lifetime.ShouldBe(lifetime, $"Service {typeof(TService).Name} has incorrect lifetime.");
    }
}