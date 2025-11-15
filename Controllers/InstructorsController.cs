using Microsoft.AspNetCore.Mvc;
using SchoolDomain.Core.Entities;
using SchoolDomain.Core.Repositories;

namespace ComunidadApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorsController : ControllerBase
{
    private readonly IInstructorRepository _repository;

    public InstructorsController(IInstructorRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var instructors = await _repository.GetAllAsync();
            return Ok(instructors);
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
            var instructor = await _repository.GetByIdAsync(id);
            if (instructor == null)
                return NotFound(new { message = "Instructor not found" });

            return Ok(instructor);
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
            var instructors = await _repository.GetByDepartmentAsync(departmentId);
            return Ok(instructors);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Instructor instructor)
    {
        try
        {
            if (instructor == null)
                return BadRequest(new { message = "Instructor cannot be null" });

            await _repository.AddAsync(instructor);
            return CreatedAtAction(nameof(GetById), new { id = instructor.Id }, instructor);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Instructor instructor)
    {
        try
        {
            var existingInstructor = await _repository.GetByIdAsync(id);
            if (existingInstructor == null)
                return NotFound(new { message = "Instructor not found" });

            instructor.Id = id;
            await _repository.UpdateAsync(instructor);
            return Ok(new { message = "Instructor updated successfully" });
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
            var instructor = await _repository.GetByIdAsync(id);
            if (instructor == null)
                return NotFound(new { message = "Instructor not found" });

            await _repository.DeleteAsync(id);
            return Ok(new { message = "Instructor deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
