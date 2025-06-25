using SnapToTable.Application.Constants;
using SnapToTable.Application.DTOs;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysisRequest;

public static class RecipeAnalysisDataFactory
{
    public static Stream CreateStream(long sizeInBytes) => new MemoryStream(new byte[sizeInBytes]);
    public static Stream CreateValidStream() => CreateStream(1024); // 1KB
    public static Stream CreateEmptyStream() => CreateStream(0);
    public static Stream CreateOversizedStream() => CreateStream(FileValidationConstants.MaxImageSizeInBytes + 1);

    public static ImageInput CreateValidImageInput(string contentType = "image/jpeg")
    {
        return new ImageInput(CreateValidStream(), contentType);
    }
    public static ImageInput CreateEmptyStreamImageInput(string contentType = "image/jpeg")
    {
        return new ImageInput(CreateEmptyStream(), contentType);
    }
}