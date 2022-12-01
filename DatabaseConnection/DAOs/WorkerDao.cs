using System.ComponentModel.Design;
using Application.DAOInterfaces;
using Domain.DTOs.JavaDTOs;
using Domain.DTOs.SearchParameters;
using Domain.Models;
using RabbitMQ;

namespace DatabaseConnection.DAOs;

public class WorkerDao : IWorkerDao
{
    private readonly Sender sender;
    private readonly Receiver receiver;
    
    public WorkerDao(Sender sender, Receiver receiver)
    {
        this.sender = sender;
        this.receiver = receiver;
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

    public async Task<Worker> GetByIdAsync(int workerId)
    {
        try
        {
            sender.GetWorkerById(workerId);

            object obj = Task.FromResult(await receiver.Receive("GetWorkerById"));
            WorkerJavaDto dto = (WorkerJavaDto) obj;

            Worker worker = new Worker(
                dto.firstName,
                dto.lastName,
                dto.phoneNumber,
                dto.mail,
                dto.address
            );
            worker.WorkerId = dto.workerId;

            return worker;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not get worker");
        }
    }

    public Task<Worker> GetByFullNameAsync(string fullName)
    {
        throw new NotImplementedException();
    }

    public async Task<Worker> UpdateAsync(Worker toUpdate)
    {

        try
        {
            sender.EditWorker(toUpdate);

            object obj = Task.FromResult(await receiver.Receive("EditWorker"));
            WorkerJavaDto dto = (WorkerJavaDto) obj;

            Worker worker = new Worker(
                dto.firstName,
                dto.lastName,
                dto.phoneNumber,
                dto.mail,
                dto.address
            );
            worker.WorkerId = dto.workerId;

            return worker;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not update worker");
        }
    }
    
    public Task DeleteAsync(int workerId)
    {
        try
        {
            sender.RemoveWorker(workerId);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not delete worker");
        }
    }
}