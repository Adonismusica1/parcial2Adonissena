namespace SchoolDomain.Dtos;

/// <summary>
/// DTO para crear un nuevo estudiante
/// </summary>
public class CreateStudentDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public DateTime EnrollmentDate { get; set; }
}

/// <summary>
/// DTO para actualizar un estudiante
/// </summary>
public class UpdateStudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}

/// <summary>
/// DTO para respuesta de estudiante
/// </summary>
public class StudentResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
