using Domain.DTOs.Worker;
using Domain.Models;

namespace HttpClients.Interfaces;

public interface IWorkerService
{
 
    Task<Worker> CreateAsync(WorkerCreationDto dto);
    Task<IEnumerable<Worker>> GetAsync(string? nameContains = null);
    Task UpdateAsync(WorkerUpdateDto dto);
    Task DeleteAsync(int id);

}