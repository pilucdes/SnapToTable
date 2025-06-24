using FluentValidation;
using SnapToTable.Application.Constants;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Validators;

public class ImageInputValidator : AbstractValidator<ImageInput>
{
    public ImageInputValidator()
    {
        RuleFor(x => x.Content)
            .NotNull().WithMessage("Image stream cannot be null.");

        RuleFor(x => x.Content.Length)
            .LessThanOrEqualTo(FileValidationConstants.MaxImageSizeInBytes)
            .WithMessage($"Image size cannot exceed {FileValidationConstants.MaxImageSizeInBytes / 1024 / 1024}MB.");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(contentType => FileValidationConstants.ValidImageTypes.Contains(contentType))
            .WithMessage("File must be a valid image type ('image/jpeg', 'image/png', 'image/webp').");
    }
}