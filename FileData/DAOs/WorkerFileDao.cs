using Application.DAOInterfaces;
using Domain.DTOs.SearchParameters;
using Domain.Models;
using RabbitMQ;

namespace FileData.DAOs;

public class WorkerFileDao : IWorkerDao
{
    // # Fields
    private readonly FileContext _context;

    // ¤ Constructor
    public WorkerFileDao(FileContext context)
    {
        _context = context;
    }
    
    // ¤ Create new Worker
    public Task<Worker> CreateAsync(Worker worker)
    {
        int workerId = 1;
        
        // * Check if there exists more workers
        if (_context.Workers.Any())
        {
            workerId = _context.Workers.Max(u => u.WorkerId);
            workerId++;
        }
        
        // * Assign Worker the correct Id
        worker.WorkerId = workerId;
        
        // * Save the new worker in the file (temp)
        _context.Workers.Add(worker);
        _context.SaveChanges();

        return Task.FromResult(worker);
    }

    // ¤ Get Worker (Search Parameters)
    public Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchParameters)
    {
        IEnumerable<Worker> workers = _context.Workers.AsEnumerable();
        if (searchParameters.WorkerName != null)
        {
            workers = _context.Workers.Where(u => u.getFullName().Contains(searchParameters.WorkerName,StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(workers);
    }

    // ¤ Get by Id
    public Task<Worker?> GetByIdAsync(int workerId)
    {
        Worker? toFind = null;

        foreach (var worker in _context.Workers)
        {
            if (worker.WorkerId == workerId)
            {
                toFind = worker;
            }
        }

        return Task.FromResult(toFind);
    }

    // ¤ Get by FullName
    public Task<Worker?> GetByFullNameAsync(string fullName)
    {
        Worker? toFind = null;

        foreach (var worker in _context.Workers)
        {
            if (worker.getFullName().Equals(fullName,StringComparison.OrdinalIgnoreCase))
            {
                toFind = worker;
            }
        }

        return Task.FromResult(toFind);
    }
    
    // ¤ Delete Worker
    public Task DeleteAsync(int workerId)
    {
        // * Check if a shift with the id exists
        Worker? existing = _context.Workers.FirstOrDefault(worker => worker.WorkerId == workerId);
        if (existing == null)
        {
            throw new Exception($"A Worker with id {workerId} does not exist!");
        }
        
        // * Remove the existing
        _context.Workers.Remove(existing);
        
        // * Save changes
        _context.SaveChanges();

        return Task.CompletedTask;
    }
}