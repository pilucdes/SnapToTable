using FluentValidation.TestHelper;
using SnapToTable.Application.Features.Recipe.GetById;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.Recipe.GetById;

public class GetRecipeByIdQueryValidatorTests
{
    private readonly GetRecipeByIdQueryValidator _validator;

    public GetRecipeByIdQueryValidatorTests()
    {
        _validator = new GetRecipeByIdQueryValidator();
    }

    [Fact]
    public void Validate_WhenIdIsEmpty_ShouldHaveValidationError()
    {
        var query = new GetRecipeByIdQuery(Guid.Empty);
        
        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id cannot be empty");
    }

    [Fact]
    public void Validate_WhenIdIsProvided_ShouldNotHaveValidationError()
    {
        var query = new GetRecipeByIdQuery(Guid.NewGuid());
        
        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
