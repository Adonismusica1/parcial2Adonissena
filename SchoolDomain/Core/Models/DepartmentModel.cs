namespace SchoolDomain.Core.Models;

public class DepartmentModel : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? HeadInstructorId { get; set; }
}
