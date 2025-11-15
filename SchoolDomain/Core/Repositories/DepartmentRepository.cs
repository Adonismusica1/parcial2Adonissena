namespace SchoolDomain.Core.Repositories;

using Microsoft.EntityFrameworkCore;
using SchoolDomain.Core.Entities;
using SchoolDomain.Core.Exceptions;
using SchoolDomain.Infrastructure;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly SchoolContext _context;

    public DepartmentRepository(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments
            .Include(d => d.HeadInstructor)
            .Include(d => d.Courses)
            .Include(d => d.Instructors)
            .Include(d => d.Students)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Include(d => d.HeadInstructor)
            .Include(d => d.Courses)
            .Include(d => d.Instructors)
            .Include(d => d.Students)
            .ToListAsync();
    }

    public async Task AddAsync(Department department)
    {
        if (string.IsNullOrEmpty(department.Name))
            throw new DepartmentException("El nombre del departamento es requerido");

        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department)
    {
        _context.Departments.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var department = await GetByIdAsync(id);
        if (department != null)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
