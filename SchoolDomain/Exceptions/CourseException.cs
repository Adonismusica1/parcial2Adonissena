namespace SchoolDomain.Exceptions;

/// <summary>
/// Excepción para errores relacionados con Cursos
/// </summary>
public class CourseException : DomainException
{
    public CourseException(string message) : base(message) { }

    public CourseException(string message, List<string> errors) 
        : base(message, errors) { }

    public static CourseException NotFound(int id)
        => new($"Curso con ID {id} no encontrado");

    public static CourseException DuplicateName(string name)
        => new($"El curso '{name}' ya existe");

    public static CourseException InvalidCredits(int credits)
        => new($"Los créditos {credits} no son válidos. Deben estar entre 1 y 10");

    public static CourseException InvalidData(List<string> errors)
        => new("El curso contiene datos inválidos", errors);
}
