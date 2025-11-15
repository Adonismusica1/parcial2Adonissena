namespace SchoolDomain.Core.Repositories;

using SchoolDomain.Core.Entities;

public interface IInstructorRepository
{
    Task<Instructor?> GetByIdAsync(int id);
    Task<IEnumerable<Instructor>> GetAllAsync();
    Task<IEnumerable<Instructor>> GetByDepartmentAsync(int departmentId);
    Task AddAsync(Instructor instructor);
    Task UpdateAsync(Instructor instructor);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
