namespace SchoolDomain.Core.Repositories;

using Microsoft.EntityFrameworkCore;
using SchoolDomain.Core.Entities;
using SchoolDomain.Core.Exceptions;
using SchoolDomain.Infrastructure;

public class CourseRepository : ICourseRepository
{
    private readonly SchoolContext _context;

    public CourseRepository(SchoolContext context)
    {
        _context = context;
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        return await _context.Courses
            .Include(c => c.Department)
            .Include(c => c.Instructor)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        return await _context.Courses
            .Include(c => c.Department)
            .Include(c => c.Instructor)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.Courses
            .Where(c => c.DepartmentId == departmentId)
            .Include(c => c.Instructor)
            .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetByInstructorAsync(int instructorId)
    {
        return await _context.Courses
            .Where(c => c.InstructorId == instructorId)
            .Include(c => c.Department)
            .ToListAsync();
    }

    public async Task AddAsync(Course course)
    {
        if (string.IsNullOrEmpty(course.Code))
            throw new CourseException("El código del curso es requerido");
        
        if (string.IsNullOrEmpty(course.Name))
            throw new CourseException("El nombre del curso es requerido");

        _context.Courses.Add(course);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Course course)
    {
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var course = await GetByIdAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
