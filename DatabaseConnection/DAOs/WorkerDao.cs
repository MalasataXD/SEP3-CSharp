using Application.DAOInterfaces;
using Domain.DTOs.JavaDTOs;
using Domain.DTOs.SearchParameters;
using Domain.Models;
using RabbitMQ;

namespace DatabaseConnection.DAOs;

public class WorkerDao : IWorkerDao
{
    private readonly Sender sender;
    
    public WorkerDao(Sender sender)
    {
        this.sender = sender;
    }
    
    public Task<Worker> CreateAsync(Worker worker)
    {
        try
        {
            sender.CreateWorker(worker);
            return Task.FromResult(worker);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not create worker");
        }
    }

    public Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchParameters)
    {
        throw new NotImplementedException();
    }

    public Task<Worker?> GetByIdAsync(int workerId)
    {
        throw new NotImplementedException();
    }

    public Task<Worker?> GetByFullNameAsync(string fullName)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int workerId)
    {
        throw new NotImplementedException();
    }
}