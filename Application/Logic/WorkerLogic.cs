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
        
        await ValidateWorker(worker);
        
        return await _workerDao.CreateAsync(worker);
    }

    public async Task<IEnumerable<Worker>> GetAsync(SearchWorkerParametersDto searchWorkerParametersDto)
    {
       return await _workerDao.GetAsync(searchWorkerParametersDto);
    }

    public async Task UpdateAsync(WorkerUpdateDto toUpdate)
    {
            Worker? worker = await _workerDao.GetByIdAsync(toUpdate.WorkerId);

            if (worker == null)
            {
                throw new Exception("Worker does not exist!");
            }

            string firstName = toUpdate.FirstName ?? worker.FirstName;
            string lastName = toUpdate.LastName ?? worker.LastName;
            int phoneNumber = toUpdate.PhoneNumber ?? worker.PhoneNumber;
            string mail = toUpdate.Mail ?? worker.Mail;
            string adress = toUpdate.Adress ?? worker.Address;

            Worker updatedWorker = new(firstName, lastName,phoneNumber,mail,adress);
            updatedWorker.WorkerId = worker.WorkerId;

            await ValidateWorker(updatedWorker);
            
            await _workerDao.UpdateAsync(updatedWorker);
        }


    public async Task DeleteAsync(int workerId)
    {
        Worker? workShift = await _workerDao.GetByIdAsync(workerId);
        if (workShift == null)
        {
            throw new Exception("Workshift does not exist!");
        }

        await _workerDao.DeleteAsync(workerId);
    }

    private async Task ValidateWorker(Worker worker)
    {
        SearchWorkerParametersDto dto = new("");
        IEnumerable<Worker> workers = await _workerDao.GetAsync(dto);

        //todo add unitTesting!
        ValidateName(worker.FirstName, worker.LastName, workers, worker.WorkerId);
        ValidateMail(worker.Mail, workers, worker.WorkerId);
        ValidatePhoneNumber(worker.PhoneNumber, workers, worker.WorkerId);
        ValidateAddress(worker.Address);
    }

    private void ValidateName(string firstName, string lastName, IEnumerable<Worker> workers, int workerId)
    {
        string[] lastNames = lastName.Split(" ");
        
        if (!Regex.IsMatch(firstName, @"^[a-åA-Å]+$"))
        {
            throw new Exception("Firstname may only contain letters");
        }

        foreach (var name in lastNames)
        {
            if (!Regex.IsMatch(name, @"^[a-åA-Å]+$"))
            {
                throw new Exception("Lastname may only contain letters");
            }
        }

        foreach (var worker in workers)
        {
            if (string.Equals(worker.getFullName(),firstName + " " + lastName) && workerId != worker.WorkerId)
            {
                throw new Exception("Two employees cannot have the same name");
            }
        }
        
    }
    private void ValidatePhoneNumber(int phoneNumber, IEnumerable<Worker> workers, int workerId)
    {
        if (phoneNumber.ToString().Length != 8)
        {
            throw new Exception("Phonenumber needs to be 8 numbers long");
        }
        
        foreach (var worker in workers)
        {
            if (phoneNumber == worker.PhoneNumber && worker.WorkerId != workerId)
            {
                throw new Exception("Two employees cannot have the same phonenumber");
            }
        }
        
    }
    private void ValidateMail(string mail, IEnumerable<Worker> workers, int workerId)
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
        
        foreach (var worker in workers)
        {
            if (string.Equals(worker.Mail,mail) && worker.WorkerId != workerId)
            {
                throw new Exception("Two employees cannot have the same mail");
            }
        }
        
    }
    private void ValidateAddress(string address)
    {
        if (!address.Contains(" "))
        {
            throw new Exception("Invalid address name");
        }
        
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