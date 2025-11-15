namespace SchoolDomain.Core.Repositories;

using Microsoft.EntityFrameworkCore;
using SchoolDomain.Core.Entities;
using SchoolDomain.Infrastructure;

public class StudentRepository : IStudentRepository
{
    private readonly SchoolContext _context;

    public StudentRepository(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        return await _context.Students
            .Include(s => s.Department)
            .Include(s => s.Courses)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Student>> GetAllAsync()
    {
        return await _context.Students
            .Include(s => s.Department)
            .Include(s => s.Courses)
            .ToListAsync();
    }

    public async Task<IEnumerable<Student>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Students
            .Where(s => s.DepartmentId == departmentId)
            .Include(s => s.Courses)
            .ToListAsync();
    }

    public async Task AddAsync(Student student)
    {
        if (string.IsNullOrEmpty(student.FirstName))
            throw new ArgumentException("First name is required");
        
        if (string.IsNullOrEmpty(student.Email))
            throw new ArgumentException("Email is required");

        _context.Students.Add(student);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Student student)
    {
        _context.Students.Update(student);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var student = await GetByIdAsync(id);
        if (student != null)
        {
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
