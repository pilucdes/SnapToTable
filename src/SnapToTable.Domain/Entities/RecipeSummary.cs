namespace SnapToTable.Domain.Entities;

public record RecipeSummary(
    Guid RecipeAnalysisId,
    string Name,
    string Category,
    string Url,
    IReadOnlyList<string> Ingredients) : BaseEntity;