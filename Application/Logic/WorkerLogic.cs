using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.SearchParameters;
using Domain.DTOs.Worker;
using Domain.Models;

namespace Application.Logic;

public class WorkerLogic : IWorkerLogic
{
    
    private readonly IWorkerDao workerDao;

    public WorkerLogic(IWorkerDao workerDao)
    {
        this.workerDao = workerDao;
    }
    
    
    public async Task<Worker> CreateAsync(WorkerCreationDto toCreate)
    {

        Worker worker = new Worker
        {
            FirstName = toCreate.FirstName,
            LastName = toCreate.LastName,
            Phone = toCreate.Phone,
            Mail = toCreate.Mail,
            Address = toCreate.Address
        };
        
        Worker? exists = await workerDao.GetByFullNameAsync(worker.getFullName());
        if (exists == null)
        {
            throw new Exception("Worker already exists!");
        }

        return await workerDao.CreateAsync(worker);
    }

    public async Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchWorkerParametersDto)
    {
        workerDao.GetAsync()
        
        throw new NotImplementedException();
    }
    
}