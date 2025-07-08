using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Contracts;

public interface IAiRecipeExtractionService
{
    Task<IReadOnlyList<RecipeExtractionResultDto>> GetRecipeFromImagesAsync(IEnumerable<ImageInputDto> images, CancellationToken cancellationToken);
} 