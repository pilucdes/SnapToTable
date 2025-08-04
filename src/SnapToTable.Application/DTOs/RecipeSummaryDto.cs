namespace SnapToTable.Application.DTOs;
public record RecipeSummaryDto(
    Guid Id,
    DateTime CreatedAt,
    Guid RecipeAnalysisId,
    string Name,
    string Category,
    string Url,
    IReadOnlyList<string> Ingredients
);