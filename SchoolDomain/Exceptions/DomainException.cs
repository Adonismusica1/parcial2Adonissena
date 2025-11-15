namespace SchoolDomain.Exceptions;

/// <summary>
/// Excepción base para todas las excepciones de dominio
/// </summary>
public class DomainException : Exception
{
    public List<string> Errors { get; set; } = new();

    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception innerException) 
        : base(message, innerException) { }

    public DomainException(string message, List<string> errors) 
        : base(message)
    {
        Errors = errors;
    }
}
