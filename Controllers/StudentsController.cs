using Microsoft.AspNetCore.Mvc;
using SchoolDomain.Core.Entities;
using SchoolDomain.Core.Repositories;

namespace ComunidadApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _repository;

    public StudentsController(IStudentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var students = await _repository.GetAllAsync();
            return Ok(students);
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
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            return Ok(student);
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
            var students = await _repository.GetByDepartmentAsync(departmentId);
            return Ok(students);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Student student)
    {
        try
        {
            if (student == null)
                return BadRequest(new { message = "Student cannot be null" });

            await _repository.AddAsync(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Student student)
    {
        try
        {
            var existingStudent = await _repository.GetByIdAsync(id);
            if (existingStudent == null)
                return NotFound(new { message = "Student not found" });

            student.Id = id;
            await _repository.UpdateAsync(student);
            return Ok(new { message = "Student updated successfully" });
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
            var student = await _repository.GetByIdAsync(id);
            if (student == null)
                return NotFound(new { message = "Student not found" });

            await _repository.DeleteAsync(id);
            return Ok(new { message = "Student deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
