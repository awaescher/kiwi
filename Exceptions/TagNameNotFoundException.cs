namespace kiwiread.Exceptions;

public class TagNotFoundException : KiwiException
{
    public TagNotFoundException(string? tag, Exception? innerException) : base($"Tag name \"{tag}\" could not be found.", innerException)
    {
    }
}
