using Shouldly;
using SnapToTable.API.Extensions;
using Xunit;

namespace SnapToTable.API.UnitTests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("PropertyName", "propertyName")]
    [InlineData("Name", "name")]
    [InlineData("ID", "iD")]
    [InlineData("URL", "uRL")]
    [InlineData("A", "a")]
    [InlineData("AlreadyCamelCase", "alreadyCamelCase")]
    public void ToCamelCase_WhenStringStartsWithUppercase_ShouldConvertToCamelCase(string input, string expected)
    {
        // Act
        var result = input.ToCamelCase();

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void ToCamelCase_WhenStringIsNullOrEmpty_ShouldReturnAsIs(string? input)
    {
        // Act
        var result = input?.ToCamelCase();

        // Assert
        result.ShouldBe(input);
    }

    [Theory]
    [InlineData("alreadyCamelCase")]
    [InlineData("name")]
    [InlineData("a")]
    public void ToCamelCase_WhenStringAlreadyStartsWithLowercase_ShouldReturnAsIs(string input)
    {
        // Act
        var result = input.ToCamelCase();

        // Assert
        result.ShouldBe(input);
    }
}