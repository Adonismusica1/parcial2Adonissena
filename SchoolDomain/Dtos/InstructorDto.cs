namespace SchoolDomain.Dtos;

/// <summary>
/// DTO para crear un nuevo instructor
/// </summary>
public class CreateInstructorDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public DateTime HireDate { get; set; }
}

/// <summary>
/// DTO para actualizar un instructor
/// </summary>
public class UpdateInstructorDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}

/// <summary>
/// DTO para respuesta de instructor
/// </summary>
public class InstructorResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string EmployeeId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime HireDate { get; set; }
    public int CourseCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
