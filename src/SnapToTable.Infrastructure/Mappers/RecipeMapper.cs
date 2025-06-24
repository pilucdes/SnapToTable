using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.DTOs;
using System.Text.RegularExpressions;

namespace SnapToTable.Infrastructure.Mappers;

public static class RecipeMapper
{
    public static RecipeExtractionResult ToExtractionResult(RawRecipeDto rawRecipe)
    {
        return new RecipeExtractionResult
        {
            Name = rawRecipe.Name,
            Category = rawRecipe.Category,
            Servings = ParseServings(rawRecipe.Servings),
            PrepTime = ParseTime(rawRecipe.PrepTime),
            CookTime = ParseTime(rawRecipe.CookTime),
            Ingredients = rawRecipe.Ingredients,
            AdditionalTime = ParseTime(rawRecipe.AdditionalTime),
            Directions = rawRecipe.Directions,
            Notes = rawRecipe.Notes
        };
    }

    private static int? ParseServings(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var match = Regex.Match(input, @"\d+");
        return match.Success && int.TryParse(match.Value, out var result) ? result : null;
    }
    private static TimeSpan? ParseTime(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var totalMinutes = 0;
        // Case-insensitive matching for "hour" or "hr"
        var hourMatches = Regex.Matches(input, @"(\d+)\s*(hour|hr)", RegexOptions.IgnoreCase);
        if (hourMatches.Count > 0 && int.TryParse(hourMatches[0].Groups[1].Value, out var hours))
        {
            totalMinutes += hours * 60;
        }

        // Case-insensitive matching for "minute" or "min"
        var minuteMatches = Regex.Matches(input, @"(\d+)\s*(minute|min)", RegexOptions.IgnoreCase);
        if (minuteMatches.Count > 0 && int.TryParse(minuteMatches[0].Groups[1].Value, out var minutes))
        {
            totalMinutes += minutes;
        }
        
        // Fallback for number-only strings, assuming minutes.
        if (totalMinutes == 0 && int.TryParse(Regex.Match(input, @"\d+").Value, out var minutesOnly))
        {
            totalMinutes = minutesOnly;
        }

        return totalMinutes > 0 ? TimeSpan.FromMinutes(totalMinutes) : null;
    }
}