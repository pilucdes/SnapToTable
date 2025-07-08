using SnapToTable.Application.DTOs;
using SnapToTable.Infrastructure.DTOs;
using System.Text.RegularExpressions;

namespace SnapToTable.Infrastructure.Mappers;

public static partial class RecipeMapper
{
    public static RecipeExtractionResultDto ToExtractionResult(RawRecipeDto rawRecipe)
    {
        return new RecipeExtractionResultDto
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

        var match = ServingsRegex().Match(input);
        return match.Success && int.TryParse(match.Value, out var result) ? result : null;
    }
    private static TimeSpan? ParseTime(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var totalMinutes = 0;

        totalMinutes += GetTotalMinutesFromHoursInput(input);
        totalMinutes += GetTotalMinutesFromMinuteInput(input);
        
        return totalMinutes > 0 ? TimeSpan.FromMinutes(totalMinutes) : null;
    }

    private static int GetTotalMinutesFromMinuteInput(string input)
    {
        var totalMinutes = 0;
        var minuteMatches = MinuteRegex().Matches(input);
        if (minuteMatches.Count > 0 && int.TryParse(minuteMatches[0].Groups[1].Value, out var minutes))
        {
            totalMinutes = minutes;
        }

        return totalMinutes;
    }
    private static int GetTotalMinutesFromHoursInput(string input)
    {
        var totalMinutes = 0;
        var hourMatches = HourRegex().Matches(input);
        if (hourMatches.Count > 0 && int.TryParse(hourMatches[0].Groups[1].Value, out var hours))
        {
            totalMinutes = hours * 60;
        }

        return totalMinutes;
    }

    [GeneratedRegex(@"(\d+)\s*(hour|hr)", RegexOptions.IgnoreCase)]
    private static partial Regex HourRegex();
    
    [GeneratedRegex(@"(\d+)\s*(minute|min)", RegexOptions.IgnoreCase)]
    private static partial Regex MinuteRegex();
    
    [GeneratedRegex(@"\d+")]
    private static partial Regex ServingsRegex();
}