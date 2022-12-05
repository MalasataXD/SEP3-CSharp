using System.ComponentModel.Design;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    
    public async Task<Worker> CreateAsync(Worker worker)
    {
        try
        {
            sender.CreateWorker(worker);
            object obj = await receiver.Receive("CreateWorker");
            WorkerJavaDto? dto = JsonSerializer.Deserialize<WorkerJavaDto>((JsonElement)obj);

            Worker newWorker = new Worker(
                dto.firstName,
                dto.lastName,
                dto.phoneNumber,
                dto.mail,
                dto.address
            );
            newWorker.WorkerId = dto.workerId;

            return newWorker;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not create worker");
        }
    }

    public async Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchParameters)
    {
        try
        {
            sender.GetWorkerBySearchParameters(new SearchWorkerParametersJavaDto(searchParameters));

            object obj = await receiver.Receive("GetWorkerBySearchParameters");
            Console.WriteLine("From server obj: " + obj);
            
            List<WorkerJavaDto>? dto = JsonSerializer.Deserialize<List<WorkerJavaDto>>((JsonElement)obj);

            Console.WriteLine(dto);
            List<Worker> result = new List<Worker>();

            foreach (var item in dto)
            {
                Worker worker = new Worker(
                    item.firstName,
                    item.lastName,
                    item.phoneNumber,
                    item.mail,
                    item.address
                );
                worker.WorkerId = item.workerId; 
                result.Add(worker);
            }
            return result.AsEnumerable();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not get worker");
        }
    }

    public async Task<Worker> GetByIdAsync(int workerId)
    {
        try
        {
            sender.GetWorkerById(workerId);

            object obj = await receiver.Receive("GetWorkerById");
            Console.WriteLine("From server obj: " + obj);
            
            WorkerJavaDto? dto = JsonSerializer.Deserialize<WorkerJavaDto>((JsonElement)obj);
            
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

    public async Task<Worker> GetByFullNameAsync(string fullName)
    {
        try
        {
            sender.GetWorkerByFullName(fullName);

            object obj = await receiver.Receive("GetWorkerByFullName");
            Console.WriteLine("From server obj: " + obj);
            
            WorkerJavaDto? dto = JsonSerializer.Deserialize<WorkerJavaDto>((JsonElement)obj);
            
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

    public async Task<Worker> UpdateAsync(Worker toUpdate)
    {

        try
        {
            sender.EditWorker(toUpdate);

            object obj = await receiver.Receive("EditWorker");
            WorkerJavaDto? dto = JsonSerializer.Deserialize<WorkerJavaDto>((JsonElement)obj);;

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