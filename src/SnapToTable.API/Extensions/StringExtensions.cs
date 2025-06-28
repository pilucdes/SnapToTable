namespace SnapToTable.API.Extensions;

public static class StringExtensions
{
    public static string ToCamelCase(this string name)
    {
        if (string.IsNullOrEmpty(name) || char.IsLower(name, 0))
            return name;
        
        return char.ToLowerInvariant(name[0]) + name[1..];
    }
}