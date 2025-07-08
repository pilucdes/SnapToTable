namespace SnapToTable.Domain.Entities;

public record RecipeAnalysis(
    ICollection<Recipe> Recipes
) : BaseEntity;