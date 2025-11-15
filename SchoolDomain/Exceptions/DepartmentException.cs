namespace SchoolDomain.Exceptions;

/// <summary>
/// Excepción para errores relacionados con Departamentos
/// </summary>
public class DepartmentException : DomainException
{
    public DepartmentException(string message) : base(message) { }

    public DepartmentException(string message, List<string> errors) 
        : base(message, errors) { }

    public static DepartmentException NotFound(int id)
        => new($"Departamento con ID {id} no encontrado");

    public static DepartmentException DuplicateName(string name)
        => new($"El departamento '{name}' ya existe");

    public static DepartmentException InvalidBudget(decimal budget)
        => new($"El presupuesto {budget} no es válido. Debe ser mayor a 0");

    public static DepartmentException InvalidData(List<string> errors)
        => new("El departamento contiene datos inválidos", errors);
}
