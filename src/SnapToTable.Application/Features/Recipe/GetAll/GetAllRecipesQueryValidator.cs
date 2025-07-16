using FluentValidation;
using SnapToTable.Application.Validators;

namespace SnapToTable.Application.Features.Recipe.GetAll;

public class GetAllRecipesQueryValidator : AbstractValidator<GetAllRecipesQuery>
{
    public GetAllRecipesQueryValidator()
    {
        RuleFor(query => query)
            .SetValidator(new PaginationValidator());
    }
}