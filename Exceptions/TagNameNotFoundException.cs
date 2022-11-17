namespace kiwipush.Exceptions;

public class TagNameNotFoundException : KiwiException
{
    public TagNameNotFoundException(string? tagName, Exception? innerException) : base($"Tag name \"{tagName}\" could not be found.", innerException)
    {
    }
}
