using Microsoft.AspNetCore.Mvc;
using SchoolDomain.Core.Entities;
using SchoolDomain.Core.Repositories;

namespace ComunidadApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _repository;

    public DepartmentsController(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var departments = await _repository.GetAllAsync();
            return Ok(departments);
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
            var department = await _repository.GetByIdAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            return Ok(department);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Department department)
    {
        try
        {
            if (department == null)
                return BadRequest(new { message = "Department cannot be null" });

            await _repository.AddAsync(department);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Department department)
    {
        try
        {
            var existingDept = await _repository.GetByIdAsync(id);
            if (existingDept == null)
                return NotFound(new { message = "Department not found" });

            department.Id = id;
            await _repository.UpdateAsync(department);
            return Ok(new { message = "Department updated successfully" });
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
            var department = await _repository.GetByIdAsync(id);
            if (department == null)
                return NotFound(new { message = "Department not found" });

            await _repository.DeleteAsync(id);
            return Ok(new { message = "Department deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
