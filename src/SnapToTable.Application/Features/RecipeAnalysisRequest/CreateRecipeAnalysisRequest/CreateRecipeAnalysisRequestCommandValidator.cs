using FluentValidation;
using SnapToTable.Application.Validators;

namespace SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;

public class CreateRecipeAnalysisRequestCommandValidator : AbstractValidator<CreateRecipeAnalysisRequestCommand>
{
    public CreateRecipeAnalysisRequestCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Recipe name is required")
            .MaximumLength(200)
            .WithMessage("Recipe name must not exceed 200 characters");

        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("At least one image is required.")
            .Must(images => images.Count <= 2)
            .WithMessage("A maximum of 2 images can be uploaded at a time.");
        
        RuleForEach(x => x.Images)
            .SetValidator(new ImageInputValidator());
    }
}