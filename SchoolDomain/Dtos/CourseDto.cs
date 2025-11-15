namespace SchoolDomain.Dtos;

/// <summary>
/// DTO para crear un nuevo curso
/// </summary>
public class CreateCourseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int InstructorId { get; set; }
    public int DepartmentId { get; set; }
}

/// <summary>
/// DTO para actualizar un curso
/// </summary>
public class UpdateCourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int InstructorId { get; set; }
    public int DepartmentId { get; set; }
}

/// <summary>
/// DTO para respuesta de curso
/// </summary>
public class CourseResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int InstructorId { get; set; }
    public string? InstructorName { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int StudentCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
