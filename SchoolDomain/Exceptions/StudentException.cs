namespace SchoolDomain.Exceptions;

/// <summary>
/// Excepción para errores relacionados con Estudiantes
/// </summary>
public class StudentException : DomainException
{
    public StudentException(string message) : base(message) { }

    public StudentException(string message, List<string> errors) 
        : base(message, errors) { }

    public static StudentException NotFound(int id)
        => new($"Estudiante con ID {id} no encontrado");

    public static StudentException InvalidEmail(string email)
        => new($"El email '{email}' no es válido");

    public static StudentException DuplicateEmail(string email)
        => new($"El email '{email}' ya está registrado");

    public static StudentException InvalidData(List<string> errors)
        => new("El estudiante contiene datos inválidos", errors);
}
