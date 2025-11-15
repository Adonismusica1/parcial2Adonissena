namespace SchoolDomain.Core.Entities;

public class Instructor : BaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? EmployeeId { get; set; }
    public int? DepartmentId { get; set; }
    public string? Title { get; set; }
    public DateTime HireDate { get; set; }
    
    // Relaciones
    public virtual Department? Department { get; set; }
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
