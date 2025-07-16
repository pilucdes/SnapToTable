using SnapToTable.Domain.Entities;

namespace SnapToTable.Tests.Common.Builders;

public class RecipeAnalysisBuilder
{
    private Guid _id = Guid.Empty;
    private DateTime _createdAt = DateTime.MinValue;

    public RecipeAnalysisBuilder WithId(Guid id)
    {
        _id = id;
        return this;
    }
    
    public RecipeAnalysisBuilder WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }
    
    public RecipeAnalysis Build()
    {
        var newRecipeAnalysis = new RecipeAnalysis
        {
            Id = _id,
            CreatedAt = _createdAt
        };
        
        return newRecipeAnalysis;
    }
}