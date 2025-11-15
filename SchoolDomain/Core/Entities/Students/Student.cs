namespace SchoolDomain.Core.Entities;

public class Student : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? StudentId { get; set; }
    public int? DepartmentId { get; set; }
    public int? SemesterLevel { get; set; }
    public DateTime EnrollmentDate { get; set; }
    
    // Relaciones
    public virtual Department? Department { get; set; }
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
