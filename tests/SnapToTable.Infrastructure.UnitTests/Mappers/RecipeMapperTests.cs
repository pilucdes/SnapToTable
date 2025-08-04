using Shouldly;
using SnapToTable.Infrastructure.DTOs;
using SnapToTable.Infrastructure.Mappers;
using Xunit;

namespace SnapToTable.Infrastructure.UnitTests.Mappers;

public class RecipeMapperTests
{
    [Fact]
    public void ToExtractionResult_ShouldMapAllPropertiesCorrectly_WhenGivenValidInput()
    {
        // Arrange
        var rawDto = new RawRecipeDto
        {
            Name = "Classic Pancakes",
            Category = "Breakfast",
            Servings = "4 servings",
            PrepTime = "10 minutes",
            Url = "https://localhost/image.webp",
            CookTime = "1 hour 20 minutes",
            AdditionalTime = "5 min",
            Ingredients = ["1 cup flour", "1 egg", "1 cup milk"],
            Directions = ["Mix ingredients", "Cook on griddle"],
            Notes = ["Serve with maple syrup.", "Can be frozen."]
        };

        // Act
        var result = RecipeMapper.ToExtractionResult(rawDto);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Classic Pancakes");
        result.Category.ShouldBe("Breakfast");
        result.Url.ShouldBe("https://localhost/image.webp");
        result.Servings.ShouldBe(4);
        result.PrepTime.ShouldBe(TimeSpan.FromMinutes(10));
        result.CookTime.ShouldBe(TimeSpan.FromMinutes(80)); // 1 hour 20 mins
        result.AdditionalTime.ShouldBe(TimeSpan.FromMinutes(5));
        result.Ingredients.ShouldBe(rawDto.Ingredients);
        result.Directions.ShouldBe(rawDto.Directions);
        result.Notes.ShouldBe(rawDto.Notes);
    }

    [Fact]
    public void ToExtractionResult_ShouldHandleDefaultAndInvalidValuesGracefully()
    {
        // Arrange
        var rawDto = new RawRecipeDto
        {
            Name = "Minimal Recipe",
            Servings = "Varies", // Not a number
            PrepTime = "A little while" // Not a parseable time
        };

        // Act
        var result = RecipeMapper.ToExtractionResult(rawDto);

        // Assert
        result.ShouldNotBeNull();
        result.Name.ShouldBe("Minimal Recipe");
        result.Category.ShouldBeEmpty();
        result.Url.ShouldBeEmpty();
        result.Servings.ShouldBeNull();
        result.PrepTime.ShouldBeNull();
        result.CookTime.ShouldBeNull();
        result.AdditionalTime.ShouldBeNull();
        result.Ingredients.ShouldBeEmpty();
        result.Directions.ShouldBeEmpty();
        result.Notes.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("4 servings", 4)]
    [InlineData("Makes about 12", 12)]
    [InlineData("Serves 6-8 people", 6)]
    [InlineData("8", 8)]
    [InlineData("No servings specified", null)]
    [InlineData(null, null)]
    [InlineData("", null)]
    [InlineData("  ", null)]
    public void ToExtractionResult_ShouldParseServingsCorrectly(string? servingsInput, int? expectedServings)
    {
        // Arrange
        var rawDto = new RawRecipeDto { Servings = servingsInput ?? string.Empty };

        // Act
        var result = RecipeMapper.ToExtractionResult(rawDto);

        // Assert
        result.Servings.ShouldBe(expectedServings);
    }

    [Theory]
    [InlineData("30 minutes", 30)]
    [InlineData("45 min", 45)]
    [InlineData("1 hour", 60)]
    [InlineData("2 hours", 120)]
    [InlineData("1hr", 60)]
    [InlineData("2 hours 15 minutes", 135)]
    [InlineData("1 hr 10 min", 70)]
    [InlineData("25 min 1 hour", 85)]
    [InlineData("Takes a while", null)]
    [InlineData(null, null)]
    [InlineData("", null)]
    public void ToExtractionResult_ShouldParseTimeCorrectly(string? timeInput, int? expectedTotalMinutes)
    {
        // Arrange
        var rawDto = new RawRecipeDto { PrepTime = timeInput ?? string.Empty };
        var expectedTimeSpan = expectedTotalMinutes.HasValue
            ? TimeSpan.FromMinutes(expectedTotalMinutes.Value)
            : (TimeSpan?)null;

        // Act
        var result = RecipeMapper.ToExtractionResult(rawDto);

        // Assert
        result.PrepTime.ShouldBe(expectedTimeSpan);
    }
}