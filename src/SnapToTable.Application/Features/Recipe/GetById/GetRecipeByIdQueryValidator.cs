using FluentValidation;

namespace SnapToTable.Application.Features.Recipe.GetById;

public class GetRecipeByIdQueryValidator : AbstractValidator<GetRecipeByIdQuery>
{
    public GetRecipeByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty().WithMessage("Id cannot be empty");

    }
}