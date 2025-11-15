using SchoolDomain.Contract;
using SchoolDomain.Core.Base;
using SchoolDomain.Core.Repositories;
using SchoolDomain.Core.Entities;
using SchoolDomain.Dtos;
using Exceptions = SchoolDomain.Exceptions;

namespace SchoolDomain.Services;

/// <summary>
/// Servicio de cursos con validaciones
/// </summary>
public class CourseService : BaseService, ICourseService
{
    private readonly ICourseRepository _repository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IInstructorRepository _instructorRepository;

    public CourseService(
        ICourseRepository repository,
        IDepartmentRepository departmentRepository,
        IInstructorRepository instructorRepository)
    {
        _repository = repository;
        _departmentRepository = departmentRepository;
        _instructorRepository = instructorRepository;
    }

    public async Task<BaseResult<CourseResponseDto>> CreateAsync(CreateCourseDto dto)
    {
        try
        {
            var errors = ValidateCreateCourseDto(dto);
            if (errors.Count > 0)
                throw Exceptions.CourseException.InvalidData(errors);

            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
                throw Exceptions.CourseException.InvalidData(new List<string> { "El departamento no existe" });

            var instructor = await _instructorRepository.GetByIdAsync(dto.InstructorId);
            if (instructor == null)
                throw Exceptions.CourseException.InvalidData(new List<string> { "El instructor no existe" });

            var course = new Course
            {
                Name = dto.Name,
                Description = dto.Description,
                Credits = dto.Credits,
                InstructorId = dto.InstructorId,
                DepartmentId = dto.DepartmentId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            await _repository.AddAsync(course);
            return BaseResult<CourseResponseDto>.Success(
                MapToResponseDto(course, instructor.FirstName + " " + instructor.LastName, department.Name));
        }
        catch (Exceptions.CourseException ex)
        {
            return BaseResult<CourseResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<CourseResponseDto>.Failure($"Error al crear curso: {ex.Message}");
        }
    }

    public async Task<BaseResult<CourseResponseDto>> GetByIdAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var course = await _repository.GetByIdAsync(id);
            
            if (course == null)
                throw Exceptions.CourseException.NotFound(id);

            var instructor = course.InstructorId.HasValue ? await _instructorRepository.GetByIdAsync(course.InstructorId.Value) : null;
            var department = course.DepartmentId.HasValue ? await _departmentRepository.GetByIdAsync(course.DepartmentId.Value) : null;

            return BaseResult<CourseResponseDto>.Success(
                MapToResponseDto(course, instructor?.FirstName + " " + instructor?.LastName, department?.Name));
        }
        catch (Exceptions.CourseException ex)
        {
            return BaseResult<CourseResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<CourseResponseDto>.Failure($"Error al obtener curso: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<CourseResponseDto>>> GetAllAsync()
    {
        try
        {
            var courses = await _repository.GetAllAsync();
            var dtos = new List<CourseResponseDto>();

            foreach (var course in courses)
            {
                var instructor = course.InstructorId.HasValue ? await _instructorRepository.GetByIdAsync(course.InstructorId.Value) : null;
                var department = course.DepartmentId.HasValue ? await _departmentRepository.GetByIdAsync(course.DepartmentId.Value) : null;
                dtos.Add(MapToResponseDto(course, instructor?.FirstName + " " + instructor?.LastName, department?.Name));
            }

            return BaseResult<List<CourseResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<CourseResponseDto>>.Failure($"Error al obtener cursos: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<CourseResponseDto>>> GetByDepartmentAsync(int departmentId)
    {
        try
        {
            ValidatePositive(departmentId, nameof(departmentId));
            var courses = await _repository.GetByDepartmentAsync(departmentId);
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            var dtos = new List<CourseResponseDto>();

            foreach (var course in courses)
            {
                var instructor = course.InstructorId.HasValue ? await _instructorRepository.GetByIdAsync(course.InstructorId.Value) : null;
                dtos.Add(MapToResponseDto(course, instructor?.FirstName + " " + instructor?.LastName, department?.Name));
            }

            return BaseResult<List<CourseResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<CourseResponseDto>>.Failure($"Error al obtener cursos: {ex.Message}");
        }
    }

    public async Task<BaseResult<List<CourseResponseDto>>> GetByInstructorAsync(int instructorId)
    {
        try
        {
            ValidatePositive(instructorId, nameof(instructorId));
            var courses = await _repository.GetByInstructorAsync(instructorId);
            var instructor = await _instructorRepository.GetByIdAsync(instructorId);
            var dtos = new List<CourseResponseDto>();

            foreach (var course in courses)
            {
                var department = course.DepartmentId.HasValue ? await _departmentRepository.GetByIdAsync(course.DepartmentId.Value) : null;
                dtos.Add(MapToResponseDto(course, instructor?.FirstName + " " + instructor?.LastName, department?.Name));
            }

            return BaseResult<List<CourseResponseDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return BaseResult<List<CourseResponseDto>>.Failure($"Error al obtener cursos: {ex.Message}");
        }
    }

    public async Task<BaseResult<CourseResponseDto>> UpdateAsync(UpdateCourseDto dto)
    {
        try
        {
            var errors = ValidateUpdateCourseDto(dto);
            if (errors.Count > 0)
                throw Exceptions.CourseException.InvalidData(errors);

            var course = await _repository.GetByIdAsync(dto.Id);
            if (course == null)
                throw Exceptions.CourseException.NotFound(dto.Id);

            course.Name = dto.Name;
            course.Description = dto.Description;
            course.Credits = dto.Credits;
            course.InstructorId = dto.InstructorId;
            course.DepartmentId = dto.DepartmentId;
            course.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(course);

            var instructor = course.InstructorId.HasValue ? await _instructorRepository.GetByIdAsync(course.InstructorId.Value) : null;
            var department = course.DepartmentId.HasValue ? await _departmentRepository.GetByIdAsync(course.DepartmentId.Value) : null;

            return BaseResult<CourseResponseDto>.Success(
                MapToResponseDto(course, instructor?.FirstName + " " + instructor?.LastName, department?.Name));
        }
        catch (Exceptions.CourseException ex)
        {
            return BaseResult<CourseResponseDto>.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult<CourseResponseDto>.Failure($"Error al actualizar curso: {ex.Message}");
        }
    }

    public async Task<BaseResult> DeleteAsync(int id)
    {
        try
        {
            ValidatePositive(id, nameof(id));
            var course = await _repository.GetByIdAsync(id);
            
            if (course == null)
                throw Exceptions.CourseException.NotFound(id);

            await _repository.DeleteAsync(id);
            return BaseResult.Success($"Curso {id} eliminado correctamente");
        }
        catch (Exceptions.CourseException ex)
        {
            return BaseResult.Failure(ex.Message, ex.Errors);
        }
        catch (Exception ex)
        {
            return BaseResult.Failure($"Error al eliminar curso: {ex.Message}");
        }
    }

    private List<string> ValidateCreateCourseDto(CreateCourseDto dto)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(dto.Name))
            errors.Add("El nombre del curso es requerido");

        if (string.IsNullOrWhiteSpace(dto.Description))
            errors.Add("La descripción del curso es requerida");

        if (dto.Credits < 1 || dto.Credits > 10)
            errors.Add("Los créditos deben estar entre 1 y 10");

        if (dto.InstructorId <= 0)
            errors.Add("El instructor es requerido");

        if (dto.DepartmentId <= 0)
            errors.Add("El departamento es requerido");

        return errors;
    }

    private List<string> ValidateUpdateCourseDto(UpdateCourseDto dto)
    {
        var errors = ValidateCreateCourseDto(new CreateCourseDto
        {
            Name = dto.Name,
            Description = dto.Description,
            Credits = dto.Credits,
            InstructorId = dto.InstructorId,
            DepartmentId = dto.DepartmentId
        });

        if (dto.Id <= 0)
            errors.Add("El ID del curso es inválido");

        return errors;
    }

    private CourseResponseDto MapToResponseDto(Course course, string? instructorName = null, string? departmentName = null)
    {
        return new CourseResponseDto
        {
            Id = course.Id,
            Name = course.Name ?? string.Empty,
            Description = course.Description ?? string.Empty,
            Credits = course.Credits,
            InstructorId = course.InstructorId ?? 0,
            InstructorName = instructorName,
            DepartmentId = course.DepartmentId ?? 0,
            DepartmentName = departmentName,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        };
    }
}
