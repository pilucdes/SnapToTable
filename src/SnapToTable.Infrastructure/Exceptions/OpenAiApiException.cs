namespace SnapToTable.Infrastructure.Exceptions;

public class OpenAiApiException : Exception
{
    public OpenAiApiException(string message) : base(message)
    {
    }
}