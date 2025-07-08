namespace SnapToTable.API.DTOs;

public record CreateRecipeAnalysisRequestDto(
    IReadOnlyList<IFormFile> Images
);