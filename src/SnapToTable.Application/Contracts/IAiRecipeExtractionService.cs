using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Contracts;

public interface IAiRecipeExtractionService
{
    Task<IReadOnlyList<RecipeExtractionResult>> GetRecipeFromImagesAsync(IEnumerable<ImageInput> images, CancellationToken cancellationToken);
} 