namespace SnapToTable.Application.DTOs;
public record RecipeDto(
    Guid Id,
    DateTime CreatedAt,
    Guid RecipeAnalysisId,
    string Name,
    string Category,
    TimeSpan? PrepTime,
    TimeSpan? CookTime,
    TimeSpan? AdditionalTime,
    int? Servings,
    IReadOnlyList<string> Ingredients,
    IReadOnlyList<string> Directions,
    IReadOnlyList<string> Notes
);