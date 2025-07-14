using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SnapToTable.Domain.Repositories;
using SnapToTable.Infrastructure.Data.Configuration;
using SnapToTable.Infrastructure.Repositories;
using SnapToTable.Application.Contracts;
using SnapToTable.Infrastructure.Extensions;
using SnapToTable.Infrastructure.Services;

namespace SnapToTable.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenAiServices(configuration);
        
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = sp.GetRequiredService<MongoDbSettings>();
            return MongoClientConfiguration.CreateClient(settings);
        });

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var settings = sp.GetRequiredService<MongoDbSettings>();
            return client.GetDatabase(settings.DatabaseName);
        });
        
        services.AddScoped<IRecipeAnalysisRepository, RecipeAnalysisRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<IAiRecipeExtractionService, AiRecipeExtractionService>();

        return services;
    }
}