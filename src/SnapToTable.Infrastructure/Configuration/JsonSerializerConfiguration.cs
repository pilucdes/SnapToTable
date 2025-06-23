using System.Text.Json;

namespace SnapToTable.Infrastructure.Configuration;

public static class JsonSerializerConfiguration
{
    public static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };
} 