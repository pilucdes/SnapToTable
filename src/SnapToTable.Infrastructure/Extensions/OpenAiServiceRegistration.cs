using Betalgo.Ranul.OpenAI.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapToTable.Infrastructure.Configuration;

namespace SnapToTable.Infrastructure.Extensions;

public static class OpenAiServiceRegistration
{
    public static IServiceCollection AddOpenAiServices(this IServiceCollection services, IConfiguration configuration)
    {
        string? openApiKey = configuration.GetSection("OpenAI:ApiKey").Value;

        if (string.IsNullOrWhiteSpace(openApiKey))
        {
            throw new InvalidOperationException("Configuration Error: The OpenAI API Key is missing or empty.");
        }

        services.Configure<OpenAiSettings>(configuration.GetSection("OpenAI"));
        services.AddOpenAIService(settings => { settings.ApiKey = openApiKey; });
        
        return services;
    }
}