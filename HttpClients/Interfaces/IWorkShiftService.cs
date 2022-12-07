using Domain.DTOs.WorkShift;
using Domain.Models;

namespace HttpClients.Interfaces;

public interface IWorkShiftService
{
    Task<WorkShift> CreateAsync(WorkShiftCreationDto dto);
    Task<IEnumerable<WorkShift>> GetAsync(string? date = null, string? workerName = null);
    Task UpdateAsync(WorkShiftUpdateDto dto);
    Task DeleteAsync(int id);
    Task ValidateAsync(WorkShiftValidateDto dto);

    Task CreateAsync(IEnumerable<WorkShiftCreationDto> dtos);
    Task UpdateAsync(IEnumerable<WorkShiftUpdateDto> dtos);
    Task DeleteAsync(List<int> ids);


}