using SchoolDomain.Core.Base;
using SchoolDomain.Dtos;

namespace SchoolDomain.Contract;

/// <summary>
/// Interfaz para el servicio de departamentos
/// </summary>
public interface IDepartmentService
{
    Task<BaseResult<DepartmentResponseDto>> CreateAsync(CreateDepartmentDto dto);
    Task<BaseResult<DepartmentResponseDto>> GetByIdAsync(int id);
    Task<BaseResult<List<DepartmentResponseDto>>> GetAllAsync();
    Task<BaseResult<DepartmentResponseDto>> UpdateAsync(UpdateDepartmentDto dto);
    Task<BaseResult> DeleteAsync(int id);
}
