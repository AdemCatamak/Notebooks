namespace Notebooks.Domain.Exceptions;

public class NotFoundException : BaseException
{
    public NotFoundException(string entityName) : base($"{entityName} not found.")
    {
    }
}