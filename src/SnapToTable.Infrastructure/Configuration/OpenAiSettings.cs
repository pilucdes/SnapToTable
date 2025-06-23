namespace SnapToTable.Infrastructure.Configuration;

public record OpenAiSettings()
{
    public int Token { get; set; } = 4096;
    public string Model { get; set; } = "gpt-4.1";
    public required string ApiKey { get; set; }
}; 