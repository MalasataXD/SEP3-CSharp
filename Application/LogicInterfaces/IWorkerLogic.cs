using Domain.DTOs;
using Domain.DTOs.SearchParameters;
using Domain.DTOs.Worker;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IWorkerLogic
{
    Task<Worker> CreateAsync(WorkerCreationDto toCreate);
    Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchWorkerParametersDto);
}