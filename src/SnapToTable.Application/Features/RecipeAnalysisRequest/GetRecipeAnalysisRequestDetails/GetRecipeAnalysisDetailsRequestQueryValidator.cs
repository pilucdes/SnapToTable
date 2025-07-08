using FluentValidation;

namespace SnapToTable.Application.Features.RecipeAnalysisRequest.GetRecipeAnalysisRequestDetails;

public class GetRecipeAnalysisDetailsRequestQueryValidator : AbstractValidator<GetRecipeAnalysisDetailsRequestQuery>
{
    public GetRecipeAnalysisDetailsRequestQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id cannot be empty");

    }
}