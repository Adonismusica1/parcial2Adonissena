namespace SchoolDomain.Core.Repositories;

using SchoolDomain.Core.Entities;

public interface IStudentRepository
{
    Task<Student?> GetByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task<IEnumerable<Student>> GetByDepartmentAsync(int departmentId);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
    Task SaveChangesAsync();
}
