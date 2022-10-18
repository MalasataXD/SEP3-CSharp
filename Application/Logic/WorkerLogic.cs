using System.Text.RegularExpressions;
using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.SearchParameters;
using Domain.DTOs.Worker;
using Domain.Models;

namespace Application.Logic;

public class WorkerLogic : IWorkerLogic
{
    
    private readonly IWorkerDao _workerDao;

    public WorkerLogic(IWorkerDao workerDao)
    {
        _workerDao = workerDao;
    }
    
    
    public async Task<Worker> CreateAsync(WorkerCreationDto toCreate)
    {
        Worker worker = new Worker(toCreate.FirstName, toCreate.LastName, toCreate.PhoneNumber, toCreate.Mail,
            toCreate.Address);
        
        ValidateWorker(worker);
        
        Worker? exists = await _workerDao.GetByFullNameAsync(worker.getFullName());
        if (exists != null)
        {
            throw new Exception("Worker already exists!");
        }
        
        return await _workerDao.CreateAsync(worker);
    }

    public async Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchWorkerParametersDto)
    {
       return await _workerDao.GetAsync(searchWorkerParametersDto);
    }

    private void ValidateWorker(Worker worker)
    {
        //todo add unitTesting!
        ValidateName(worker.FirstName, worker.LastName);
        ValidatePhoneNumber(worker.PhoneNumber);
        ValidateMail(worker.Mail);
        ValidateAddress(worker.Address);
    }

    private void ValidateName(string firstName, string lastName)
    {
        if (!(Regex.IsMatch(firstName, @"^[a-zA-Z]+$") && Regex.IsMatch(lastName, @"^[a-zA-Z]+$")))
        {
            throw new Exception("Name may only contain letters");
        }
    }
    private void ValidatePhoneNumber(int phoneNumber)
    {
        if (phoneNumber.ToString().Length != 8)
        {
            throw new Exception("Phonenumber needs to be 8 numbers long");
        }
    }
    
    private void ValidateMail(string mail)
    {
        try
        {
            string[] temp = mail.Split("@");
            string[] temp2 = mail.Split(".");


            if (!temp2.Last().Equals("dk") && !temp2.Last().Equals("com"))
            {
                throw new Exception("Email must end with .dk or .com");
            }
        }
        catch (Exception e)
        {
            throw new Exception("Invalid mail");
        }
    }
    
    private void ValidateAddress(string address)
    {
        string[] temp = address.Split(" ");
        
        for (int i = 0; i < temp.Length-1; i++)
        {
            if (!Regex.IsMatch(temp[i], @"^[a-zA-Z]+$"))
            {
                throw new Exception("Invalid address name");
            }
        }

        if (!Regex.IsMatch(temp.Last(), @"^[a-zA-Z0-9]+$"))
        {
            throw new Exception("Invalid address number");
        }
    }


    
}