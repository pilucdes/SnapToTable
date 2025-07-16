using FluentValidation.TestHelper;
using Shouldly;
using SnapToTable.Application.Features.Recipe.GetAll;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.Recipe.GetAll;

public class GetAllRecipesQueryValidatorTests
{
    private readonly GetAllRecipesQueryValidator _validator;

    public GetAllRecipesQueryValidatorTests()
    {
        _validator = new GetAllRecipesQueryValidator();
    }

    [Fact]
    public void Should_Not_Have_Error_When_Query_Is_Valid()
    {
        // Arrange
        var query = new GetAllRecipesQuery(null,null,Page: 1, PageSize: 20);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Not_Have_Error_When_PageSize_Is_Exactly_100()
    {
        // Arrange
        var query = new GetAllRecipesQuery(null,null,Page: 1, PageSize: 100);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Page_Is_Not_Greater_Than_Zero(int invalidPage)
    {
        // Arrange
        var query = new GetAllRecipesQuery(null,null,Page: invalidPage, PageSize: 10);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Page)
            .WithErrorMessage("Page must be greater than 0");
    }

    [Fact]
    public void Should_Have_Error_When_PageSize_Is_Greater_Than_100()
    {
        // Arrange
        var query = new GetAllRecipesQuery(null,null,Page: 1, PageSize: 101);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        var error = result.ShouldHaveValidationErrorFor(q => q.PageSize)
            .FirstOrDefault();

        // Assert
        error.ShouldNotBeNull();
        error.ErrorMessage.ShouldBe("Page size must be less than or equal to 100");
    }

    [Fact]
    public void Should_Have_Errors_When_Both_Page_And_PageSize_Are_Invalid()
    {
        // Arrange
        var query = new GetAllRecipesQuery(null,null,Page: 0, PageSize: 200);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.Errors.Count.ShouldBe(2);
        result.ShouldHaveValidationErrorFor(q => q.Page);
        result.ShouldHaveValidationErrorFor(q => q.PageSize);
    }
}