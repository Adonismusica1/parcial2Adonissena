using SchoolDomain.Core.Base;
using SchoolDomain.Dtos;

namespace SchoolDomain.Contract;

/// <summary>
/// Interfaz para el servicio de instructores
/// </summary>
public interface IInstructorService
{
    Task<BaseResult<InstructorResponseDto>> CreateAsync(CreateInstructorDto dto);
    Task<BaseResult<InstructorResponseDto>> GetByIdAsync(int id);
    Task<BaseResult<List<InstructorResponseDto>>> GetAllAsync();
    Task<BaseResult<List<InstructorResponseDto>>> GetByDepartmentAsync(int departmentId);
    Task<BaseResult<InstructorResponseDto>> UpdateAsync(UpdateInstructorDto dto);
    Task<BaseResult> DeleteAsync(int id);
}
