namespace SnapToTable.API.DTOs;

public record GetAllRecipesRequestDto(
    Guid? RecipeAnalysisId,
    string? Filter,
    int Page = 1,
    int PageSize = 20
);