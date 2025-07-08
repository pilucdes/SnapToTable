using FluentValidation;

namespace SnapToTable.Application.Features.RecipeAnalysis.GetById;

public class GetRecipeAnalysisByIdQueryValidator : AbstractValidator<GetRecipeAnalysisByIdQuery>
{
    public GetRecipeAnalysisByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id cannot be empty");

    }
}