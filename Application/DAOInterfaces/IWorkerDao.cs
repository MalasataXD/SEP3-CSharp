using Domain.DTOs.SearchParameters;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface IWorkerDao
{
    Task<Worker> CreateAsync(Worker worker);
    Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchParameters);
    Task<Worker?> GetByIdAsync(int workerId);
    Task<Worker?> GetByFullNameAsync(string fullName); // NOTE: Could be removed, if not needed?
}