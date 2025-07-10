using SnapToTable.Domain.Entities;

namespace SnapToTable.Tests.Common.Builders;

public class RecipeBuilder
{
    private string _name = "Default Test Recipe";
    private string _category = "Default Category";
    private TimeSpan? _prepTime = TimeSpan.FromMinutes(10);
    private TimeSpan? _cookTime = TimeSpan.FromMinutes(20);
    private TimeSpan? _additionalTime = TimeSpan.FromMinutes(30);
    private int? _servings = 4;
    private readonly List<string> _ingredients = [];
    private readonly List<string> _directions = [];
    private readonly List<string> _notes = [];
    
    public RecipeBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public RecipeBuilder WithCategory(string category)
    {
        _category = category;
        return this;
    }

    public RecipeBuilder WithPrepTime(TimeSpan? prepTime)
    {
        _prepTime = prepTime;
        return this;
    }

    public RecipeBuilder WithCookTime(TimeSpan? cookTime)
    {
        _cookTime = cookTime;
        return this;
    }
    
    public RecipeBuilder WithAdditionalTime(TimeSpan? additionalTime)
    {
        _additionalTime = additionalTime;
        return this;
    }
    
    public RecipeBuilder WithServings(int? servings)
    {
        _servings = servings;
        return this;
    }

    public RecipeBuilder WithIngredients(IEnumerable<string> ingredients)
    {
        _ingredients.AddRange(ingredients);
        return this;
    }
    
    public RecipeBuilder WithDirections(IEnumerable<string> ingredients)
    {
        _directions.AddRange(ingredients);
        return this;
    }
    
    public RecipeBuilder WithNotes(IEnumerable<string> notes)
    {
        _notes.AddRange(notes);
        return this;
    }
    public Recipe Build()
    {
        return new Recipe(
            _name,
            _category,
            _prepTime,
            _cookTime,
            _additionalTime,
            _servings,
            _ingredients,
            _directions,
            _notes
        );
    }
}