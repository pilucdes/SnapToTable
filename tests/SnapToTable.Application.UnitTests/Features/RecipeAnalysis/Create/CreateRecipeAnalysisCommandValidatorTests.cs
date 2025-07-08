using FluentValidation.TestHelper;
using Shouldly;
using SnapToTable.Application.Constants;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysis.Create;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysis.Create;

public class CreateRecipeAnalysisCommandValidatorTests
{
    private readonly CreateRecipeAnalysisCommandValidator _validator;

    public CreateRecipeAnalysisCommandValidatorTests()
    {
        _validator = new CreateRecipeAnalysisCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Images_Is_Empty()
    {
        // Arrange
        var command = new CreateRecipeAnalysisCommand(new List<ImageInputDto>());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Images)
            .WithErrorMessage("At least one image is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Image_Stream_Is_Empty()
    {
        // Arrange
        var command = new CreateRecipeAnalysisCommand(new List<ImageInputDto>
        {
            RecipeAnalysisDataFactory.CreateEmptyStreamImageInput()
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Images[0].Content.Length")
            .WithErrorMessage("Image stream cannot be empty.");
    }

    [Fact]
    public void Should_Have_Error_When_More_Than_Two_Images()
    {
        // Arrange
        var images = Enumerable.Range(0, 3)
            .Select(_ => RecipeAnalysisDataFactory.CreateValidImageInput())
            .ToList();
        
        var command = new CreateRecipeAnalysisCommand(images);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Images)
            .WithErrorMessage("A maximum of 2 images can be uploaded at a time.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void Should_Not_Have_Error_When_Valid_Number_Of_Images_Are_Provided(int imageCount)
    {
        // Arrange
        var images = Enumerable.Range(0, imageCount)
            .Select(_ => RecipeAnalysisDataFactory.CreateValidImageInput())
            .ToList();
        
        var command = new CreateRecipeAnalysisCommand(images);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Validate_Each_Image_Using_ImageInputValidator_Invalid_ContentType()
    {
        // Arrange
        var command = new CreateRecipeAnalysisCommand(new List<ImageInputDto>
        {
            new(RecipeAnalysisDataFactory.CreateValidStream(), "invalid/type")
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Images[0].ContentType")
            .WithErrorMessage("File must be a valid image type ('image/jpeg', 'image/png', 'image/webp').");
    }

    [Fact]
    public void Should_Validate_Each_Image_Using_ImageInputValidator_Oversized_Image()
    {
        // Arrange
        var oversizedStream = RecipeAnalysisDataFactory.CreateOversizedStream();
        var command = new CreateRecipeAnalysisCommand(new List<ImageInputDto>
        {
            new(oversizedStream, "image/jpeg")
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor("Images[0].Content.Length")
            .WithErrorMessage($"Image size cannot exceed {FileValidationConstants.MaxImageSizeInMegabytes}MB.");
    }

    [Theory]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    [InlineData("image/webp")]
    public void Should_Accept_Valid_Image_ContentTypes(string contentType)
    {
        // Arrange
        var command = new CreateRecipeAnalysisCommand(new List<ImageInputDto>
        {
            new(RecipeAnalysisDataFactory.CreateValidStream(), contentType)
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("text/plain")]
    [InlineData("application/pdf")]
    [InlineData("image/gif")]
    [InlineData("image/bmp")]
    public void Should_Reject_Invalid_Image_ContentTypes(string contentType)
    {
        // Arrange
        var command = new CreateRecipeAnalysisCommand(new List<ImageInputDto>
        {
            new(RecipeAnalysisDataFactory.CreateValidStream(), contentType)
        });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldContain(e => e.PropertyName == "Images[0].ContentType");
    }

}