using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.Configuration;

namespace SnapToTable.Infrastructure.UnitTests.Factories;

public static class AiRecipeExtractionDataFactory
{
    public static OpenAiSettings CreateOpenAiSettings() => new()
    {
        RecipeExtractionPrompt = "Test Prompt",
        Model = "gpt-4-vision-preview",
        Token = 4096,
        ApiKey = "ABC"
    };

    public static ImageInputDto CreateImageInput(byte[] content, string contentType = "image/jpeg") =>
        new(new MemoryStream(content), contentType);

    public static string CreateValidRecipesJson() =>
        "{\"recipes\":[{\"name\":\"Test Cake\",\"category\":\"Dessert\",\"prepTime\":\"10 minutes\",\"cookTime\":\"30 minutes\",\"additionalTime\":\"5 minutes\",\"servings\":\"8\",\"ingredients\":[\"Flour\",\"Sugar\"],\"directions\":[\"Mix\",\"Bake\"],\"notes\":[\"Enjoy\"]}]}";
}