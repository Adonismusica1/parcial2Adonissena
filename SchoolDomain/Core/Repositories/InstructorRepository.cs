namespace SchoolDomain.Core.Repositories;

using Microsoft.EntityFrameworkCore;
using SchoolDomain.Core.Entities;
using SchoolDomain.Infrastructure;

public class InstructorRepository : IInstructorRepository
{
    private readonly SchoolContext _context;

    public InstructorRepository(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Instructor?> GetByIdAsync(int id)
    {
        return await _context.Instructors
            .Include(i => i.Department)
            .Include(i => i.Courses)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Instructor>> GetAllAsync()
    {
        return await _context.Instructors
            .Include(i => i.Department)
            .Include(i => i.Courses)
            .ToListAsync();
    }

    public async Task<IEnumerable<Instructor>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Instructors
            .Where(i => i.DepartmentId == departmentId)
            .Include(i => i.Courses)
            .ToListAsync();
    }

    public async Task AddAsync(Instructor instructor)
    {
        if (string.IsNullOrEmpty(instructor.FirstName))
            throw new ArgumentException("First name is required");
        
        if (string.IsNullOrEmpty(instructor.Email))
            throw new ArgumentException("Email is required");

        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Instructor instructor)
    {
        _context.Instructors.Update(instructor);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var instructor = await GetByIdAsync(id);
        if (instructor != null)
        {
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
