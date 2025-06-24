namespace SnapToTable.Domain.Entities;

public record Recipe(
    string Name,
    string Category,
    TimeSpan? PrepTime,
    TimeSpan? CookTime,
    TimeSpan? AdditionalTime,
    int? Servings,
    IReadOnlyList<string> Ingredients,
    IReadOnlyList<string> Directions,
    IReadOnlyList<string> Notes);