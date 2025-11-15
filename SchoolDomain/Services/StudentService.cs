using SchoolDomain.Contract;
using SchoolDomain.Core.Base;
using SchoolDomain.Core.Repositories;
using SchoolDomain.Core.Entities;
using SchoolDomain.Dtos;
using SchoolDomain.Exceptions;
using System.Text.RegularExpressions;

namespace SchoolDomain.Services;

/// <summary>
/// Servicio de estudiantes con validaciones
/// </summary>
public class StudentService : BaseService, IStudentService
{
    private readonly IStudentRepository _repository;
    private readonly IDepartmentRepository _departmentRepository;

    public StudentService(IStudentRepository repository, IDepartmentRepository departmentRepository)
    {
        _repository = repository;
        _departmentRepository = departmentRepository;
    }

    public async Task<BaseResult<StudentResponseDto>> CreateAsync(CreateStudentDto dto)
    {
        try
        {
            var errors = ValidateCreateStudentDto(dto);
            if (errors.Count > 0)
                throw StudentException.InvalidData(errors);

            // Validar que el departamento exista
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                throw StudentException.NotFound(dto.DepartmentId);

            var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                StudentId = dto.StudentId,
                DepartmentId = dto.DepartmentId,
                EnrollmentDate = dto.EnrollmentDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _repository.AddAsync(student);
            return BaseResult<StudentResponseDto>.Success(MapToResponseDto(student, department.Name));
        }
        catch (StudentException ex)
        {
            return BaseResult<StudentResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<StudentResponseDto>.Failure($"Error al crear estudiante: {ex.Message}");
        }
    }

    public async Task<BaseResult<StudentResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var student = await _repository.GetByIdAsync(id);
            
            if (student == null)
                throw StudentException.NotFound(id);

            var department = student.DepartmentId.HasValue ? await _departmentRepository.GetByIdAsync(student.DepartmentId.Value) : null;
            return BaseResult<StudentResponseDto>.Success(MapToResponseDto(student, department?.Name));
        }
        catch (StudentException ex)
        {
            return BaseResult<StudentResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<StudentResponseDto>.Failure($"Error al obtener estudiante: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<StudentResponseDto>>> GetAllAsync()
    {
        try
        {
            var students = await _repository.GetAllAsync();
            var dtos = new List<StudentResponseDto>();

            foreach (var student in students)
            {
                var department = student.DepartmentId.HasValue ? await _departmentRepository.GetByIdAsync(student.DepartmentId.Value) : null;
                dtos.Add(MapToResponseDto(student, department?.Name));
            }

            return BaseResult<List<StudentResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<StudentResponseDto>>.Failure($"Error al obtener estudiantes: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<StudentResponseDto>>> GetByDepartmentAsync(int departmentId)
    {
        try
        {
            ValidatePositive(departmentId, nameof(departmentId));
            var students = await _repository.GetByDepartmentAsync(departmentId);
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            
            var dtos = students.Select(s => MapToResponseDto(s, department?.Name)).ToList();
            return BaseResult<List<StudentResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<StudentResponseDto>>.Failure($"Error al obtener estudiantes: {ex.Message}");
        }
    }

    public async Task<BaseResult<StudentResponseDto>> UpdateAsync(UpdateStudentDto dto)
    {
        try
        {
            var errors = ValidateUpdateStudentDto(dto);
            if (errors.Count > 0)
                throw StudentException.InvalidData(errors);

            var student = await _repository.GetByIdAsync(dto.Id);
            if (student == null)
                throw StudentException.NotFound(dto.Id);

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                throw StudentException.NotFound(dto.DepartmentId);

            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Email = dto.Email;
            student.StudentId = dto.StudentId;
            student.DepartmentId = dto.DepartmentId;
            student.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(student);
            return BaseResult<StudentResponseDto>.Success(MapToResponseDto(student, department.Name));
        }
        catch (StudentException ex)
        {
            return BaseResult<StudentResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<StudentResponseDto>.Failure($"Error al actualizar estudiante: {ex.Message}");
        }
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var student = await _repository.GetByIdAsync(id);
            
            if (student == null)
                throw StudentException.NotFound(id);

            await _repository.DeleteAsync(id);
            return BaseResult.Success($"Estudiante {id} eliminado correctamente");
        }
        catch (StudentException ex)
        {
            return BaseResult.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult.Failure($"Error al eliminar estudiante: {ex.Message}");
        }
    }

    public async Task<BaseResult<bool>> EnrollInCourseAsync(int studentId, int courseId)
    {
        try
        {
            ValidatePositive(studentId, nameof(studentId));
            ValidatePositive(courseId, nameof(courseId));

            var student = await _repository.GetByIdAsync(studentId);
            if (student == null)
                throw StudentException.NotFound(studentId);

            return BaseResult<bool>.Success(true, "Estudiante inscrito en el curso");
        }
        catch (StudentException ex)
        {
            return BaseResult<bool>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<bool>.Failure($"Error al inscribir estudiante: {ex.Message}");
        }
    }

    private List<string> ValidateCreateStudentDto(CreateStudentDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.FirstName))
            errors.Add("El nombre del estudiante es requerido");

        if (string.IsNullOrWhiteSpace(dto.LastName))
            errors.Add("El apellido del estudiante es requerido");

        if (!IsValidEmail(dto.Email))
            errors.Add("El email no tiene un formato válido");

        if (string.IsNullOrWhiteSpace(dto.StudentId))
            errors.Add("El ID del estudiante es requerido");

        if (dto.DepartmentId <= 0)
            errors.Add("El departamento es requerido");

        if (dto.EnrollmentDate == default)
            errors.Add("La fecha de inscripción es requerida");

        if (dto.EnrollmentDate > DateTime.Now)
            errors.Add("La fecha de inscripción no puede ser en el futuro");

        return errors;
    }

    private List<string> ValidateUpdateStudentDto(UpdateStudentDto dto)
    {
        var errors = ValidateCreateStudentDto(new CreateStudentDto
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            StudentId = dto.StudentId,
            DepartmentId = dto.DepartmentId,
            EnrollmentDate = DateTime.Now
        });

        if (dto.Id <= 0)
            errors.Add("El ID del estudiante es inválido");

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

    private StudentResponseDto MapToResponseDto(Student student, string? departmentName = null)
    {
        return new StudentResponseDto
        {
            Id = student.Id,
            FirstName = student.FirstName ?? string.Empty,
            LastName = student.LastName ?? string.Empty,
            Email = student.Email ?? string.Empty,
            StudentId = student.StudentId ?? string.Empty,
            DepartmentId = student.DepartmentId ?? 0,
            DepartmentName = departmentName,
            EnrollmentDate = student.EnrollmentDate,
            CreatedAt = student.CreatedAt,
            UpdatedAt = student.UpdatedAt
        };
    }
}
