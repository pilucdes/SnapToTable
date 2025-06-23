using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.Contracts;

public interface IRecipeExtractionService
{
    Task<IReadOnlyList<RecipeExtractionResult>> GetRecipeFromImagesAsync(IEnumerable<ImageInput> images, CancellationToken cancellationToken);
} 