namespace SnapToTable.Domain.Entities;

public record RecipeSummary(
    Guid RecipeAnalysisId,
    string Name,
    string Category,
    IReadOnlyList<string> Ingredients) : BaseEntity;