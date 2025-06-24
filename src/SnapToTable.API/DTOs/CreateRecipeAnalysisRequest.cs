namespace SnapToTable.API.DTOs;

public record CreateRecipeAnalysisRequest(
    IReadOnlyList<IFormFile> Images
);