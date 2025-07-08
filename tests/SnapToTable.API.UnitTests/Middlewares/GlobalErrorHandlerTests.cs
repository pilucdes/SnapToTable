using System.Text.Json;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using SnapToTable.API.Middlewares;
using Xunit;

namespace SnapToTable.API.UnitTests.Middlewares;

public class GlobalErrorHandlerTests
{
    private readonly Mock<RequestDelegate> _mockNext;
    private readonly Mock<ILogger<GlobalErrorHandler>> _mockLogger;
    private readonly Mock<IHostEnvironment> _mockHostEnvironment;
    private readonly GlobalErrorHandler _middleware;

    public GlobalErrorHandlerTests()
    {
        _mockNext = new Mock<RequestDelegate>();
        _mockLogger = new Mock<ILogger<GlobalErrorHandler>>();
        _mockHostEnvironment = new Mock<IHostEnvironment>();
        _middleware =
            new GlobalErrorHandler(_mockNext.Object, _mockLogger.Object, _mockHostEnvironment.Object);
    }

    [Fact]
    public async Task InvokeAsync_WhenNoException_CallsNextDelegateAndDoesNotModifyResponse()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.StatusCode = 200;

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        _mockNext.Verify(next => next(context), Times.Once);
        context.Response.StatusCode.ShouldBe(200);
        _mockLogger.Verify(
            x => x.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
            Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_WhenGenericExceptionInProduction_Returns500ProblemDetails()
    {
        // Arrange
        var exception = new InvalidOperationException("Something went wrong.");
        _mockHostEnvironment.Setup(env => env.EnvironmentName).Returns("Production");
        _mockNext.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.ShouldBe(500);
        context.Response.ContentType.ShouldBe("application/problem+json");

        var responseBody = await GetResponseBody(context);
        var problemDetails = JsonSerializer.Deserialize<TestValidationProblemDetails>(responseBody,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(500);
        problemDetails.Title.ShouldBe("An internal server error has occurred.");
        problemDetails.Detail.ShouldBe("An unexpected error occurred. Please try again later.");
        problemDetails.Extensions.ShouldNotContainKey("exceptionType");
        problemDetails.Extensions.ShouldNotContainKey("stackTrace");

        VerifyLoggerWasCalled(exception);
    }


    [Fact]
    public async Task InvokeAsync_WhenGenericExceptionInDevelopment_Returns500WithStackTrace()
    {
        // Arrange
        var exception = new NotSupportedException("This feature is not supported.");
        _mockHostEnvironment.Setup(env => env.EnvironmentName).Returns("Development");
        _mockNext.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.ShouldBe(500);
        var responseBody = await GetResponseBody(context);
        var problemDetails = JsonSerializer.Deserialize<TestValidationProblemDetails>(responseBody);

        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(500);
        problemDetails.Title.ShouldBe("An internal server error has occurred.");
        problemDetails.Extensions["exceptionType"]?.ToString().ShouldBe("NotSupportedException");
        problemDetails.Extensions["stackTrace"]?.ToString().ShouldNotBeNullOrEmpty();
        
        VerifyLoggerWasCalled(exception);
    }

    [Fact]
    public async Task InvokeAsync_WhenValidationException_Returns400WithFormattedErrors()
    {
        // Arrange
        _mockHostEnvironment.Setup(env => env.EnvironmentName).Returns("Production");
        var validationFailures = new List<ValidationFailure>
        {
            new("FirstName", "First name is required."),
            new("EmailAddress", "A valid email is required."),
            new("EmailAddress", "Email address must be unique.")
        };
        var exception = new ValidationException("Validation Failed", validationFailures);
        _mockNext.Setup(next => next(It.IsAny<HttpContext>())).ThrowsAsync(exception);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await _middleware.InvokeAsync(context);

        // Assert
        context.Response.StatusCode.ShouldBe(400);
        context.Response.ContentType.ShouldBe("application/problem+json");

        var responseBody = await GetResponseBody(context);
        var problemDetails = JsonSerializer.Deserialize<TestValidationProblemDetails>(responseBody);

        problemDetails.ShouldNotBeNull();
        problemDetails.Status.ShouldBe(400);
        problemDetails.Title.ShouldBe("One or more validation errors occurred.");
        problemDetails.Extensions.ShouldNotContainKey("exceptionType");
        problemDetails.Extensions.ShouldNotContainKey("stackTrace");
        
        var errorDict = problemDetails.Errors;
        errorDict.Keys.ShouldContain("firstName");
        errorDict["firstName"].ShouldHaveSingleItem("First name is required.");

        errorDict.Keys.ShouldContain("emailAddress");
        errorDict["emailAddress"].Length.ShouldBe(2);
        errorDict["emailAddress"].ShouldContain("A valid email is required.");
        errorDict["emailAddress"].ShouldContain("Email address must be unique.");


        VerifyLoggerWasCalled(exception);
    }

    private async Task<string> GetResponseBody(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(context.Response.Body);
        return await reader.ReadToEndAsync();
    }

    private void VerifyLoggerWasCalled(Exception expectedException)
    {
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An unhandled exception has occurred:")),
                expectedException,
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)!),
            Times.Once);
    }
    
    public class TestValidationProblemDetails : ProblemDetails
    {
        [JsonPropertyName("errors")]
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}