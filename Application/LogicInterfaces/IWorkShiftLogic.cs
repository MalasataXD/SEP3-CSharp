using Domain.DTOs;
using Domain.DTOs.SearchParameters;
using Domain.DTOs.WorkShift;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IWorkShiftLogic
{
    Task<WorkShift> CreateAsync(WorkShiftCreationDto toCreate); // ¤ Create (Post)
    Task<IEnumerable<WorkShift>> GetAsync(SearchShiftParametersDto searchParameters); // ¤ Get
    Task<WorkShift?> GetByIdAsync(int shiftId); // ¤ Get
    Task UpdateAsync(WorkShiftUpdateDto toUpdate); // ¤ Update (Patch)
    Task DeleteAsync(int shiftId); // ¤ Delete
    void ValidateAsync(WorkShiftValidateDto toValidate); // ¤ Validate
    Task DeleteAsync(List<int> shiftIds); // ¤ Delete
    Task CreateAsync(List<WorkShiftCreationDto> shifts); // ¤ Delete
    Task UpdateAsync(List<WorkShiftUpdateDto> toUpdate); // ¤ Update (Patch)
    
}