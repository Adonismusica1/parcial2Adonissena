using SchoolDomain.Core.Base;
using SchoolDomain.Dtos;

namespace SchoolDomain.Contract;

/// <summary>
/// Interfaz para el servicio de cursos
/// </summary>
public interface ICourseService
{
    Task<BaseResult<CourseResponseDto>> CreateAsync(CreateCourseDto dto);
    Task<BaseResult<CourseResponseDto>> GetByIdAsync(int id);
    Task<BaseResult<List<CourseResponseDto>>> GetAllAsync();
    Task<BaseResult<List<CourseResponseDto>>> GetByDepartmentAsync(int departmentId);
    Task<BaseResult<List<CourseResponseDto>>> GetByInstructorAsync(int instructorId);
    Task<BaseResult<CourseResponseDto>> UpdateAsync(UpdateCourseDto dto);
    Task<BaseResult> DeleteAsync(int id);
}
