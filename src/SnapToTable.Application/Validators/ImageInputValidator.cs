using FluentValidation;
using SnapToTable.Application.Constants;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Validators;

public class ImageInputValidator : AbstractValidator<ImageInputDto>
{
    public ImageInputValidator()
    {
        RuleFor(x => x.Content.Length)
            .GreaterThan(0).WithMessage("Image stream cannot be empty.");

        RuleFor(x => x.Content.Length)
            .LessThanOrEqualTo(FileValidationConstants.MaxImageSizeInBytes)
            .WithMessage($"Image size cannot exceed {FileValidationConstants.MaxImageSizeInMegabytes}MB.");

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .Must(contentType => FileValidationConstants.ValidImageTypes.Contains(contentType))
            .WithMessage("File must be a valid image type ('image/jpeg', 'image/png', 'image/webp').");
    }
}