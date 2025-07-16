using FluentValidation;
using SnapToTable.Application.Validators;

namespace SnapToTable.Application.Features.RecipeAnalysis.Create;

public class CreateRecipeAnalysisCommandValidator : AbstractValidator<CreateRecipeAnalysisCommand>
{
    public CreateRecipeAnalysisCommandValidator()
    {
        RuleFor(query => query.Images)
            .NotEmpty().WithMessage("At least one image is required.")
            .Must(images => images.Count <= 2)
            .WithMessage("A maximum of 2 images can be uploaded at a time.");
        
        RuleForEach(query => query.Images)
            .SetValidator(new ImageInputValidator());
    }
}