namespace SchoolDomain.Core.Entities;

public class Department : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? HeadInstructorId { get; set; }
    public string? Building { get; set; }
    public string? Budget { get; set; }
    
    // Relaciones
    public virtual Instructor? HeadInstructor { get; set; }
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
