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
    
        /*
        Worker? exists = await _workerDao.GetByFullNameAsync(worker.getFullName());
        if (exists != null)
        {
            throw new Exception("Worker already exists!");
        }
        */
        
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

            ValidateWorker(updatedWorker);
            
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

    private void ValidateWorker(Worker worker)
    {
        //todo add unitTesting!
        ValidateName(worker.FirstName, worker.LastName);
        ValidateMail(worker.Mail);
        ValidatePhoneNumber(worker.PhoneNumber);
        ValidateAddress(worker.Address);
    }

    private void ValidateName(string firstName, string lastName)
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