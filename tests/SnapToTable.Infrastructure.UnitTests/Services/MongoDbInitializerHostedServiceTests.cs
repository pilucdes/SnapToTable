using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using SnapToTable.Infrastructure.Data.Configuration;
using SnapToTable.Infrastructure.Services;
using Xunit;

namespace SnapToTable.Infrastructure.UnitTests.Services;

public class MongoDbInitializerHostedServiceTests
{
     private readonly Mock<ILogger<MongoDbInitializerHostedService>> _mockLogger;
    private readonly Mock<IMongoDbIndexInitializer> _mockInitializer;
    private readonly MongoDbInitializerHostedService _hostedService;

    public MongoDbInitializerHostedServiceTests()
    {
        _mockLogger = new Mock<ILogger<MongoDbInitializerHostedService>>();
        _mockInitializer = new Mock<IMongoDbIndexInitializer>();
        
        var mockServiceProvider = new Mock<IServiceProvider>();
        var mockServiceScope = new Mock<IServiceScope>();
        var mockServiceScopeFactory = new Mock<IServiceScopeFactory>();

        mockServiceScope.Setup(s => s.ServiceProvider).Returns(mockServiceProvider.Object);
        mockServiceScopeFactory.Setup(s => s.CreateScope()).Returns(mockServiceScope.Object);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(mockServiceScopeFactory.Object);
        mockServiceProvider.Setup(sp => sp.GetService(typeof(IMongoDbIndexInitializer))).Returns(_mockInitializer.Object);

        _hostedService = new MongoDbInitializerHostedService(mockServiceProvider.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task StartAsync_WhenInitializationSucceeds_LogsInformationAndCompletes()
    {
        // Arrange
        _mockInitializer
            .Setup(i => i.CreateIndexesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _hostedService.StartAsync(CancellationToken.None);

        // Assert
        _mockInitializer.Verify(i => i.CreateIndexesAsync(), Times.Once);
        
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("initialization completed successfully")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public async Task StartAsync_WhenInitializationFails_LogsCriticalAndThrows()
    {
        // Arrange
        var expectedException = new InvalidOperationException("DB connection failed");
        _mockInitializer
            .Setup(i => i.CreateIndexesAsync())
            .ThrowsAsync(expectedException);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _hostedService.StartAsync(CancellationToken.None)
        );

        expectedException.ShouldBeEquivalentTo(exception);
        
        _mockInitializer.Verify(i => i.CreateIndexesAsync(), Times.Once);
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Critical,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                expectedException,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}