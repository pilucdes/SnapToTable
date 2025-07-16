using FluentValidation;
using SnapToTable.Application.Features.Common;

namespace SnapToTable.Application.Validators;

public class PaginationValidator : AbstractValidator<IPaginatedQuery>
{
    public PaginationValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .LessThanOrEqualTo(100)
            .WithMessage("Page size must be less than or equal to 100");
    }
}