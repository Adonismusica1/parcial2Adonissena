namespace SchoolDomain.Core.Entities;

public class Course : BaseEntity
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public int Credits { get; set; }
    public int? DepartmentId { get; set; }
    public int? InstructorId { get; set; }
    public int? MaxStudents { get; set; }
    
    // Relaciones
    public virtual Department? Department { get; set; }
    public virtual Instructor? Instructor { get; set; }
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
