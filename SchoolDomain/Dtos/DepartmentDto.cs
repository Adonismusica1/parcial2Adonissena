namespace SchoolDomain.Dtos;

/// <summary>
/// DTO para crear un nuevo departamento
/// </summary>
public class CreateDepartmentDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
}

/// <summary>
/// DTO para actualizar un departamento
/// </summary>
public class UpdateDepartmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
}

/// <summary>
/// DTO para respuesta de departamento
/// </summary>
public class DepartmentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public int CourseCount { get; set; }
    public int StudentCount { get; set; }
    public int InstructorCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
