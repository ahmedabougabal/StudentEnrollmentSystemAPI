// Application/Exceptions/ApplicationException.cs
namespace UniversityEnrollmentSystem.Application.Exceptions;

public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message) { }
}

public class NotFoundException : ApplicationException
{
    public NotFoundException(string name, object key) 
        : base($"Entity \"{name}\" ({key}) was not found.") { }
}

public class ValidationException : ApplicationException
{
    public ValidationException(string message) : base(message) { }
}