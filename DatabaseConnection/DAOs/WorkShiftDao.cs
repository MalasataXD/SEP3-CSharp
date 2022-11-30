using Application.DAOInterfaces;
using Domain.DTOs.JavaDTOs;
using Domain.DTOs.SearchParameters;
using Domain.Models;
using RabbitMQ;

namespace DatabaseConnection.DAOs;

public class WorkShiftDao : IWorkShiftDao
{
    private readonly Sender sender;

    public WorkShiftDao(Sender sender)
    {
        this.sender = sender;
    }

    public Task<WorkShift> CreateAsync(WorkShift shift)
    {
        ShiftJavaDto newShift = new ShiftJavaDto(shift);
        
        try
        {
            sender.CreateShift(newShift);
            return Task.FromResult(shift);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not create shift");
        }
    }

    public Task<IEnumerable<WorkShift>> GetAsync(SearchShiftParametersDto searchParameters)
    {
        throw new NotImplementedException();
    }
    
    public Task<WorkShift?> GetByIdAsync(int shiftId)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(WorkShift toUpdate)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int shiftId)
    {
        throw new NotImplementedException();
    }
}