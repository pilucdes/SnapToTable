using SnapToTable.Infrastructure.DTOs;

namespace SnapToTable.API.IntegrationTests.Builders;

public class RawRecipesDtoBuilder
{
    private RawRecipesDto _dto = new();

    public RawRecipesDtoBuilder()
    {
        WithDefaultRecipe();
    }

    public RawRecipesDtoBuilder WithDefaultRecipe()
    {
        _dto.Recipes =
        [
            new RawRecipeDto
            {
                Category = "Dessert",
                PrepTime = "20 minutes",
                AdditionalTime = "5 minutes",
                CookTime = "15 minutes",
                Directions = ["Direction1", "Direction2"],
                Ingredients = ["Ingredient1", "Ingredient2"],
                Notes = ["Note1", "Note2"],
                Name = "Recipe1",
                Servings = "4"
            }
        ];
        return this;
    }
    
    public RawRecipesDto Build()
    {
        return _dto;
    }
}