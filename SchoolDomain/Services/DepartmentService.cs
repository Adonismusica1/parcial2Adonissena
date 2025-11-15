using SchoolDomain.Contract;
using SchoolDomain.Core.Base;
using SchoolDomain.Core.Repositories;
using SchoolDomain.Core.Entities;
using SchoolDomain.Dtos;
using Exceptions = SchoolDomain.Exceptions;

namespace SchoolDomain.Services;

/// <summary>
/// Servicio de departamentos con validaciones
/// </summary>
public class DepartmentService : BaseService, IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResult<DepartmentResponseDto>> CreateAsync(CreateDepartmentDto dto)
    {
        try
        {
            var errors = ValidateCreateDepartmentDto(dto);
            if (errors.Count > 0)
                throw Exceptions.DepartmentException.InvalidData(errors);

            var department = new Department
            {
                Name = dto.Name,
                Description = dto.Description,
                Budget = dto.Budget.ToString(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _repository.AddAsync(department);
            return BaseResult<DepartmentResponseDto>.Success(MapToResponseDto(department));
        }
        catch (Exceptions.DepartmentException ex)
        {
            return BaseResult<DepartmentResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<DepartmentResponseDto>.Failure($"Error al crear departamento: {ex.Message}");
        }
    }

    public async Task<BaseResult<DepartmentResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var department = await _repository.GetByIdAsync(id);
            
            if (department == null)
                throw Exceptions.DepartmentException.NotFound(id);

            return BaseResult<DepartmentResponseDto>.Success(MapToResponseDto(department));
        }
        catch (Exceptions.DepartmentException ex)
        {
            return BaseResult<DepartmentResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<DepartmentResponseDto>.Failure($"Error al obtener departamento: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<DepartmentResponseDto>>> GetAllAsync()
    {
        try
        {
            var departments = await _repository.GetAllAsync();
            var dtos = departments.Select(d => MapToResponseDto(d)).ToList();
            return BaseResult<List<DepartmentResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<DepartmentResponseDto>>.Failure($"Error al obtener departamentos: {ex.Message}");
        }
    }

    public async Task<BaseResult<DepartmentResponseDto>> UpdateAsync(UpdateDepartmentDto dto)
    {
        try
        {
            var errors = ValidateUpdateDepartmentDto(dto);
            if (errors.Count > 0)
                throw Exceptions.DepartmentException.InvalidData(errors);

            var department = await _repository.GetByIdAsync(dto.Id);
            if (department == null)
                throw Exceptions.DepartmentException.NotFound(dto.Id);

            department.Name = dto.Name;
            department.Description = dto.Description;
            department.Budget = dto.Budget.ToString();
            department.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(department);
            return BaseResult<DepartmentResponseDto>.Success(MapToResponseDto(department));
        }
        catch (Exceptions.DepartmentException ex)
        {
            return BaseResult<DepartmentResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<DepartmentResponseDto>.Failure($"Error al actualizar departamento: {ex.Message}");
        }
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var department = await _repository.GetByIdAsync(id);
            
            if (department == null)
                throw Exceptions.DepartmentException.NotFound(id);

            await _repository.DeleteAsync(id);
            return BaseResult.Success($"Departamento {id} eliminado correctamente");
        }
        catch (Exceptions.DepartmentException ex)
        {
            return BaseResult.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult.Failure($"Error al eliminar departamento: {ex.Message}");
        }
    }

    private List<string> ValidateCreateDepartmentDto(CreateDepartmentDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name))
            errors.Add("El nombre del departamento es requerido");

        if (string.IsNullOrWhiteSpace(dto.Description))
            errors.Add("La descripción del departamento es requerida");

        if (dto.Budget <= 0)
            errors.Add("El presupuesto debe ser mayor a 0");

        return errors;
    }

    private List<string> ValidateUpdateDepartmentDto(UpdateDepartmentDto dto)
    {
        var errors = ValidateCreateDepartmentDto(new CreateDepartmentDto
        {
            Name = dto.Name,
            Description = dto.Description,
            Budget = dto.Budget
        });

        if (dto.Id <= 0)
            errors.Add("El ID del departamento es inválido");

        return errors;
    }

    private DepartmentResponseDto MapToResponseDto(Department department)
    {
        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name ?? string.Empty,
            Description = department.Description ?? string.Empty,
            Budget = decimal.TryParse(department.Budget, out var parsedBudget) ? parsedBudget : 0m,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }
}
