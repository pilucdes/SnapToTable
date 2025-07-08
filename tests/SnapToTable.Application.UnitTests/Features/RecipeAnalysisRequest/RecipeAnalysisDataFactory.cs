using SnapToTable.Application.Constants;
using SnapToTable.Application.DTOs;
using SnapToTable.Application.Features.RecipeAnalysisRequest.CreateRecipeAnalysisRequest;

namespace SnapToTable.Application.UnitTests.Features.RecipeAnalysisRequest;

public static class RecipeAnalysisDataFactory
{
    public static Stream CreateStream(long sizeInBytes) => new MemoryStream(new byte[sizeInBytes]);
    public static Stream CreateValidStream() => CreateStream(1024); // 1KB
    public static Stream CreateEmptyStream() => CreateStream(0);
    public static Stream CreateOversizedStream() => CreateStream(FileValidationConstants.MaxImageSizeInBytes + 1);

    public static ImageInputDto CreateValidImageInput(string contentType = "image/jpeg")
    {
        return new ImageInputDto(CreateValidStream(), contentType);
    }

    public static ImageInputDto CreateEmptyStreamImageInput(string contentType = "image/jpeg")
    {
        return new ImageInputDto(CreateEmptyStream(), contentType);
    }

    public static List<RecipeExtractionResultDto> CreateRecipeExtractionResults()
    {
        return
        [
            new()
            {
                Name = "Test Chocolate Cake",
                Category = "Dessert",
                PrepTime = TimeSpan.FromMinutes(15),
                CookTime = TimeSpan.FromMinutes(30),
                AdditionalTime = TimeSpan.FromMinutes(10),
                Servings = 8,
                Ingredients = ["1 cup flour", "1 cup sugar"],
                Directions = ["Mix ingredients", "Bake at 350F"],
                Notes = ["Enjoy!"]
            }
        ];
    }

    public static CreateRecipeAnalysisRequestCommand CreateValidCommand()
    {
        return new CreateRecipeAnalysisRequestCommand(new List<ImageInputDto>
        {
            CreateValidImageInput()
        });
    }
}