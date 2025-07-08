namespace SnapToTable.Application.DTOs;

public record RecipeAnalysisRequestDto(
    Guid Id,
    DateTime CreatedAt,
    List<RecipeDto> Recipes
);

// The DTO for a single recipe within the request
public record RecipeDto(
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