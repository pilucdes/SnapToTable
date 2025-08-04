namespace SnapToTable.Application.DTOs;

public class RecipeExtractionResultDto
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public TimeSpan? PrepTime { get; set; }
    public TimeSpan? CookTime { get; set; }
    public TimeSpan? AdditionalTime { get; set; }
    public int? Servings { get; set; }
    public List<string> Ingredients { get; set; } = new();
    public List<string> Directions { get; set; } = new();
    public List<string> Notes { get; set; } = new();
} 