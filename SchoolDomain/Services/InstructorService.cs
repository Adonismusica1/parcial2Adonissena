using SchoolDomain.Contract;
using SchoolDomain.Core.Base;
using SchoolDomain.Core.Repositories;
using SchoolDomain.Core.Entities;
using SchoolDomain.Dtos;
using SchoolDomain.Exceptions;
using System.Text.RegularExpressions;

namespace SchoolDomain.Services;

/// <summary>
/// Servicio de instructores con validaciones
/// </summary>
public class InstructorService : BaseService, IInstructorService
{
    private readonly IInstructorRepository _repository;
    private readonly IDepartmentRepository _departmentRepository;

    public InstructorService(IInstructorRepository repository, IDepartmentRepository departmentRepository)
    {
        _repository = repository;
        _departmentRepository = departmentRepository;
    }

    public async Task<BaseResult<InstructorResponseDto>> CreateAsync(CreateInstructorDto dto)
    {
        try
        {
            var errors = ValidateCreateInstructorDto(dto);
            if (errors.Count > 0)
                throw InstructorException.InvalidData(errors);

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                throw InstructorException.NotFound(dto.DepartmentId);

            var instructor = new Instructor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                EmployeeId = dto.EmployeeId,
                Title = dto.Title,
                DepartmentId = dto.DepartmentId,
                HireDate = dto.HireDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _repository.AddAsync(instructor);
            return BaseResult<InstructorResponseDto>.Success(MapToResponseDto(instructor, department.Name));
        }
        catch (InstructorException ex)
        {
            return BaseResult<InstructorResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<InstructorResponseDto>.Failure($"Error al crear instructor: {ex.Message}");
        }
    }

    public async Task<BaseResult<InstructorResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var instructor = await _repository.GetByIdAsync(id);
            
            if (instructor == null)
                throw InstructorException.NotFound(id);

            var department = await _departmentRepository.GetByIdAsync(instructor.DepartmentId.HasValue ? instructor.DepartmentId.Value : 0);
            return BaseResult<InstructorResponseDto>.Success(MapToResponseDto(instructor, department?.Name));
        }
        catch (InstructorException ex)
        {
            return BaseResult<InstructorResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<InstructorResponseDto>.Failure($"Error al obtener instructor: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<InstructorResponseDto>>> GetAllAsync()
    {
        try
        {
            var instructors = await _repository.GetAllAsync();
            var dtos = new List<InstructorResponseDto>();

            foreach (var instructor in instructors)
            {
                var department = await _departmentRepository.GetByIdAsync(instructor.DepartmentId.HasValue ? instructor.DepartmentId.Value : 0);
                dtos.Add(MapToResponseDto(instructor, department?.Name));
            }

            return BaseResult<List<InstructorResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<InstructorResponseDto>>.Failure($"Error al obtener instructores: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<InstructorResponseDto>>> GetByDepartmentAsync(int departmentId)
    {
        try
        {
            ValidatePositive(departmentId, nameof(departmentId));
            var instructors = await _repository.GetByDepartmentAsync(departmentId);
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            
            var dtos = instructors.Select(i => MapToResponseDto(i, department?.Name)).ToList();
            return BaseResult<List<InstructorResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<InstructorResponseDto>>.Failure($"Error al obtener instructores: {ex.Message}");
        }
    }

    public async Task<BaseResult<InstructorResponseDto>> UpdateAsync(UpdateInstructorDto dto)
    {
        try
        {
            var errors = ValidateUpdateInstructorDto(dto);
            if (errors.Count > 0)
                throw InstructorException.InvalidData(errors);

            var instructor = await _repository.GetByIdAsync(dto.Id);
            if (instructor == null)
                throw InstructorException.NotFound(dto.Id);

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                throw InstructorException.NotFound(dto.DepartmentId);

            instructor.FirstName = dto.FirstName;
            instructor.LastName = dto.LastName;
            instructor.Email = dto.Email;
            instructor.EmployeeId = dto.EmployeeId;
            instructor.Title = dto.Title;
            instructor.DepartmentId = dto.DepartmentId;
            instructor.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(instructor);
            return BaseResult<InstructorResponseDto>.Success(MapToResponseDto(instructor, department.Name));
        }
        catch (InstructorException ex)
        {
            return BaseResult<InstructorResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<InstructorResponseDto>.Failure($"Error al actualizar instructor: {ex.Message}");
        }
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var instructor = await _repository.GetByIdAsync(id);
            
            if (instructor == null)
                throw InstructorException.NotFound(id);

            await _repository.DeleteAsync(id);
            return BaseResult.Success($"Instructor {id} eliminado correctamente");
        }
        catch (InstructorException ex)
        {
            return BaseResult.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult.Failure($"Error al eliminar instructor: {ex.Message}");
        }
    }

    private List<string> ValidateCreateInstructorDto(CreateInstructorDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.FirstName))
            errors.Add("El nombre del instructor es requerido");

        if (string.IsNullOrWhiteSpace(dto.LastName))
            errors.Add("El apellido del instructor es requerido");

        if (!IsValidEmail(dto.Email))
            errors.Add("El email no tiene un formato válido");

        if (string.IsNullOrWhiteSpace(dto.EmployeeId))
            errors.Add("El ID de empleado es requerido");

        if (string.IsNullOrWhiteSpace(dto.Title))
            errors.Add("El título es requerido");

        if (dto.DepartmentId <= 0)
            errors.Add("El departamento es requerido");

        if (dto.HireDate == default)
            errors.Add("La fecha de contratación es requerida");

        if (dto.HireDate > DateTime.Now)
            errors.Add("La fecha de contratación no puede ser en el futuro");

        return errors;
    }

    private List<string> ValidateUpdateInstructorDto(UpdateInstructorDto dto)
    {
        var errors = ValidateCreateInstructorDto(new CreateInstructorDto
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            EmployeeId = dto.EmployeeId,
            Title = dto.Title,
            DepartmentId = dto.DepartmentId,
            HireDate = DateTime.Now
        });

        if (dto.Id <= 0)
            errors.Add("El ID del instructor es inválido");

        return errors;
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }
        catch
        {
            return false;
        }
    }

    private InstructorResponseDto MapToResponseDto(Instructor instructor, string? departmentName = null)
    {
        return new InstructorResponseDto
        {
            Id = instructor.Id,
            FirstName = instructor.FirstName ?? string.Empty,
            LastName = instructor.LastName ?? string.Empty,
            Email = instructor.Email ?? string.Empty,
            EmployeeId = instructor.EmployeeId ?? string.Empty,
            Title = instructor.Title ?? string.Empty,
            DepartmentId = instructor.DepartmentId ?? 0,
            DepartmentName = departmentName,
            HireDate = instructor.HireDate,
            CreatedAt = instructor.CreatedAt,
            UpdatedAt = instructor.UpdatedAt
        };
    }
}
