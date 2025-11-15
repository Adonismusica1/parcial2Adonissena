namespace SchoolDomain.Core.Models;

public class CourseModel : BaseEntity
{
    public string? Name { get; set; }
    public string? Code { get; set; }
    public int Credits { get; set; }
    public int DepartmentId { get; set; }
}
