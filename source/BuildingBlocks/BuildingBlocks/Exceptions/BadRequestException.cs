namespace BuildingBlocks.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message)
    {
    }

    public BadRequestException(string name, string key) : base($"Entity \"{name}\" ({key}) not found")
    {
    }
}