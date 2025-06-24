namespace SnapToTable.Infrastructure.Configuration;

public record OpenAiSettings()
{
    public int Token { get; init; } = 4096;
    public string Model { get; init; } = "gpt-4.1";
    public required string ApiKey { get; init; }
    public required string RecipeExtractionPrompt { get; init; } =
        "You are a helpful recipe assistant. Extract up to 3 recipes for a full meal from the attached image(s). " +
        "If information is spread across multiple images, combine it intelligently. If the image(s) doesn't contain enough " +
        "ingredients for any meal recipes return empty for recipes property. Return the data as a single JSON object. " +
        "The JSON object must have this schema : " +
        "```json\\n{\\n  \\\"recipes\\\": [\\n    {\\n      \\\"name\\\": \\\"The full name of the recipe\\\",\\n      \\\"category\\\": \\\"e.g., Dessert, Main Course, Appetizer\\\",\\n      \\\"prepTime\\\": \\\"e.g., '15 minutes'\\\",\\n      \\\"cookTime\\\": \\\"e.g., '10 minutes'\\\",\\n      \\\"additionalTime\\\": \\\"e.g., '1 hour chilling' or empty string\\\",\\n      \\\"servings\\\": \\\"e.g., '4'\\\",\\n      \\\"ingredients\\\": [\\n        \\\"full ingredient string 1\\\",\\n        \\\"full ingredient string 2\\\"\\n      ],\\n      \\\"directions\\\": [\\n        \\\"step 1\\\",\\n        \\\"step 2\\\"\\n      ],\\n      \\\"notes\\\": [\\n        \\\"note 1\\\",\\n        \\\"note 2\\\"\\n      ]\\n    }\\n  ]\\n}```\\n. " +
        "Note that no fields can be empty.";
};