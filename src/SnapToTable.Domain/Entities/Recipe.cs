namespace SnapToTable.Domain.Entities;

public record Recipe(
    Guid RecipeAnalysisId,
    string Name,
    string Category,
    string Url,
    TimeSpan? PrepTime,
    TimeSpan? CookTime,
    TimeSpan? AdditionalTime,
    int? Servings,
    IReadOnlyList<string> Ingredients,
    IReadOnlyList<string> Directions,
    IReadOnlyList<string> Notes) : BaseEntity;