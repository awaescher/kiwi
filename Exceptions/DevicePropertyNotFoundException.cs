namespace kiwipush.Exceptions;

public class PropertyNotFoundException : KiwiException
{
    public PropertyNotFoundException(string? propertyName, Exception? innerException) : base($"Property \"{propertyName}\" could not be found.", innerException)
    {
    }
}