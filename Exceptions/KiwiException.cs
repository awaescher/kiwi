namespace kiwipush.Exceptions;

public class KiwiException : Exception
{
    public KiwiException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
