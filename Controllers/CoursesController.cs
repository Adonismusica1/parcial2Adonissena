using Microsoft.AspNetCore.Mvc;
using SchoolDomain.Core.Entities;
using SchoolDomain.Core.Repositories;

namespace ComunidadApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseRepository _repository;

    public CoursesController(ICourseRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var courses = await _repository.GetAllAsync();
            return Ok(courses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            return Ok(course);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("department/{departmentId}")]
    public async Task<IActionResult> GetByDepartment(int departmentId)
    {
        try
        {
            var courses = await _repository.GetByDepartmentAsync(departmentId);
            return Ok(courses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpGet("instructor/{instructorId}")]
    public async Task<IActionResult> GetByInstructor(int instructorId)
    {
        try
        {
            var courses = await _repository.GetByInstructorAsync(instructorId);
            return Ok(courses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Course course)
    {
        try
        {
            if (course == null)
                return BadRequest(new { message = "Course cannot be null" });

            await _repository.AddAsync(course);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Course course)
    {
        try
        {
            var existingCourse = await _repository.GetByIdAsync(id);
            if (existingCourse == null)
                return NotFound(new { message = "Course not found" });

            course.Id = id;
            await _repository.UpdateAsync(course);
            return Ok(new { message = "Course updated successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var course = await _repository.GetByIdAsync(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            await _repository.DeleteAsync(id);
            return Ok(new { message = "Course deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
