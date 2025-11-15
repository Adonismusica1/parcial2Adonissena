using SchoolDomain.Core.Base;
using SchoolDomain.Dtos;

namespace SchoolDomain.Contract;

/// <summary>
/// Interfaz para el servicio de estudiantes
/// </summary>
public interface IStudentService
{
    Task<BaseResult<StudentResponseDto>> CreateAsync(CreateStudentDto dto);
    Task<BaseResult<StudentResponseDto>> GetByIdAsync(int id);
    Task<BaseResult<List<StudentResponseDto>>> GetAllAsync();
    Task<BaseResult<List<StudentResponseDto>>> GetByDepartmentAsync(int departmentId);
    Task<BaseResult<StudentResponseDto>> UpdateAsync(UpdateStudentDto dto);
    Task<BaseResult> DeleteAsync(int id);
    Task<BaseResult<bool>> EnrollInCourseAsync(int studentId, int courseId);
}
