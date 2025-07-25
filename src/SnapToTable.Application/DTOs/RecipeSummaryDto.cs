namespace SnapToTable.Application.DTOs;
public record RecipeSummaryDto(
    Guid Id,
    DateTime CreatedAt,
    Guid RecipeAnalysisId,
    string Name,
    string Category,
    IReadOnlyList<string> Ingredients
);