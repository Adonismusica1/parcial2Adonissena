namespace SchoolDomain.Core.Repositories;

using SchoolDomain.Core.Entities;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(int id);
    Task<IEnumerable<Course>> GetAllAsync();
    Task<IEnumerable<Course>> GetByDepartmentAsync(int departmentId);
    Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId);
    Task AddAsync(Course course);
    Task UpdateAsync(Course course);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
