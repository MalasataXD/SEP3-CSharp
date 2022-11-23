using Domain.DTOs.WorkShift;
using Domain.Models;

namespace HttpClients.Interfaces;

public interface IWorkShiftService
{
    Task<WorkShift> CreateAsync(WorkShiftCreationDto dto);
    Task<IEnumerable<WorkShift>> GetAsync(string? date = null, string? workerName = null);
    Task UpdateAsync(WorkShiftUpdateDto dto);


}