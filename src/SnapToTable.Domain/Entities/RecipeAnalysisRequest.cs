namespace SnapToTable.Domain.Entities;

public record RecipeAnalysisRequest(
    ICollection<Recipe> Recipes
) : BaseEntity;