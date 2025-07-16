namespace SnapToTable.API.DTOs;

public record GetAllRecipesRequestDto(
    Guid? RecipeAnalysisId,
    string? Filter
) : BasePaginatedRequestDto;