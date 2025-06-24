namespace SnapToTable.Infrastructure.Exceptions;

public class RecipeDeserializationException : Exception
{
    public RecipeDeserializationException(string message) : base(message) { }
}