using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shouldly;
using SnapToTable.Infrastructure.Configuration;
using SnapToTable.Infrastructure.Extensions;
using Xunit;

namespace SnapToTable.Infrastructure.UnitTests.Extensions;

public class OpenAiServiceRegistrationTests
{
    [Fact]
        public void AddOpenAiServices_ShouldThrowInvalidOperationException_WhenApiKeyIsMissing()
        {
            // Arrange
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().Build(); // Empty configuration

            // Act & Assert
            var exception = Should.Throw<InvalidOperationException>(() => 
                services.AddOpenAiServices(configuration)
            );

            exception.Message.ShouldContain("The OpenAI API Key is missing or empty.");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")] // Whitespace
        public void AddOpenAiServices_ShouldThrowInvalidOperationException_WhenApiKeyIsNullOrWhitespace(string? invalidApiKey)
        {
            // Arrange
            var services = new ServiceCollection();
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "OpenAI:ApiKey", invalidApiKey }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Act & Assert
            var exception = Should.Throw<InvalidOperationException>(() => 
                services.AddOpenAiServices(configuration)
            );

            exception.Message.ShouldContain("The OpenAI API Key is missing or empty.");
        }

        [Fact]
        public void AddOpenAiServices_ShouldRegisterServices_WhenApiKeyIsValid()
        {
            // Arrange
            var services = new ServiceCollection();
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "OpenAI:ApiKey", "my-secret-test-key" },
                { "OpenAI:Model", "gpt-4-test" }
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            
            // Act
            var resultServices = services.AddOpenAiServices(configuration);
            var serviceProvider = services.BuildServiceProvider();

            // Assert
            resultServices.ShouldBeSameAs(services);
            
            var options = serviceProvider.GetService<IOptions<OpenAiSettings>>();
            options.ShouldNotBeNull();
            options.Value.ApiKey.ShouldBe("my-secret-test-key");
            options.Value.Model.ShouldBe("gpt-4-test");
            
            var openAiService = serviceProvider.GetService<IOpenAIService>();
            openAiService.ShouldNotBeNull();
            openAiService.ShouldBeOfType<OpenAIService>();
        }
}