using FluentValidation.TestHelper;
using SnapToTable.Application.Features.RecipeAnalysis.GetById;
using Xunit;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysis.GetById;

public class GetRecipeAnalysisByIdQueryValidatorTests
{
    private readonly GetRecipeAnalysisByIdQueryValidator _validator;

    public GetRecipeAnalysisByIdQueryValidatorTests()
    {
        _validator = new GetRecipeAnalysisByIdQueryValidator();
    }

    [Fact]
    public void Validate_WhenIdIsEmpty_ShouldHaveValidationError()
    {
        var query = new GetRecipeAnalysisByIdQuery(Guid.Empty);
        
        var result = _validator.TestValidate(query);
        
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id cannot be empty");
    }

    [Fact]
    public void Validate_WhenIdIsProvided_ShouldNotHaveValidationError()
    {
        var query = new GetRecipeAnalysisByIdQuery(Guid.NewGuid());
        
        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
