namespace SchoolDomain.Exceptions;

/// <summary>
/// Excepción para errores relacionados con Instructores
/// </summary>
public class InstructorException : DomainException
{
    public InstructorException(string message) : base(message) { }

    public InstructorException(string message, List<string> errors) 
        : base(message, errors) { }

    public static InstructorException NotFound(int id)
        => new($"Instructor con ID {id} no encontrado");

    public static InstructorException InvalidEmail(string email)
        => new($"El email '{email}' no es válido");

    public static InstructorException DuplicateEmail(string email)
        => new($"El email '{email}' ya está registrado");

    public static InstructorException InvalidData(List<string> errors)
        => new("El instructor contiene datos inválidos", errors);
}
