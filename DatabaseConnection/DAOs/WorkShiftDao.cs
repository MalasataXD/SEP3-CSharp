using Application.DAOInterfaces;
using Domain.DTOs.JavaDTOs;
using Domain.DTOs.SearchParameters;
using Domain.Models;
using RabbitMQ;

namespace DatabaseConnection.DAOs;

public class WorkShiftDao : IWorkShiftDao
{
    private readonly Sender sender;
    private readonly Receiver receiver;

    public WorkShiftDao(Sender sender, Receiver receiver)
    {
        this.sender = sender;
        this.receiver = receiver;
    }

    public async Task<WorkShift> CreateAsync(WorkShift shift)
    {
        
        try
        {
            sender.CreateShift(shift);

            object obj = Task.FromResult(receiver.Receive("CreateShift"));
            ShiftJavaDto dto = (ShiftJavaDto) obj;

            Worker worker = new Worker("", "", 0, "", "");
            worker.WorkerId = dto.workerId;
            
            return new WorkShift(dto.date, dto.fromHour + ":" + dto.fromMinute, dto.toHour + ":" + dto.toMinute, worker, dto.breakAmount.ToString(), dto.bossId.ToString());
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