namespace SnapToTable.Infrastructure.DTOs;

public class RawRecipesDto
{
    public RawRecipeDto[] Recipes { get; init; } = [];
} 
public class RawRecipeDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string PrepTime { get; set; } = string.Empty;
    public string CookTime { get; set; } = string.Empty;
    public string AdditionalTime { get; set; } = string.Empty;
    public string Servings { get; set; } = string.Empty;
    public List<string> Ingredients { get; set; } = new();
    public List<string> Directions { get; set; } = new();
    public List<string> Notes { get; set; } = new();
} 