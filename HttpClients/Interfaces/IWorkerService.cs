using Domain.DTOs.Worker;
using Domain.Models;

namespace HttpClients.Interfaces;

public interface IWorkerService
{
 
    Task<Worker> CreateAsync(WorkerCreationDto dto);
    Task<IEnumerable<Worker>> GetUsersAsync(string? nameContains = null);
    
}