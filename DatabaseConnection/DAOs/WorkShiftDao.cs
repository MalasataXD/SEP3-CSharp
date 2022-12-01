using System.Security.AccessControl;
using System.Text.Json;
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
            Console.WriteLine("Dao: " + shift.Date + " " + shift.BossId + "" + shift.Worker.WorkerId);
            
            sender.CreateShift(shift);

            object obj = await receiver.Receive("CreateShift");
            ShiftJavaDto? dto = JsonSerializer.Deserialize<ShiftJavaDto>((JsonElement)obj);

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
    
    public async Task<WorkShift?> GetByIdAsync(int shiftId)
    {
        try
        {
            sender.GetShiftById(shiftId);

            object obj = await receiver.Receive("GetShiftById");
            ShiftJavaDto? dto = JsonSerializer.Deserialize<ShiftJavaDto>((JsonElement)obj);
            
            Worker worker = new Worker("", "", 0, "", "");
            worker.WorkerId = dto.workerId;

            WorkShift workShift = new WorkShift(
                dto.date,
                $"{dto.fromHour}:{dto.fromMinute}",
                $"{dto.toHour}:{dto.toMinute}",
                worker,
                dto.breakAmount.ToString(),
                dto.bossId.ToString()
            );
            workShift.ShiftId = dto.shiftId;

            return workShift;


        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<WorkShift> UpdateAsync(WorkShift toUpdate)
    {
        try
        {
            Console.WriteLine(JsonSerializer.Serialize(toUpdate));
            sender.EditShift(toUpdate);

            object obj = await receiver.Receive("EditShift");
            ShiftJavaDto? receivedObj = JsonSerializer.Deserialize<ShiftJavaDto>((JsonElement)obj);
            
            Worker worker = new Worker("", "", 0, "", "");
            worker.WorkerId = receivedObj.workerId;

            
            WorkShift workShift = new WorkShift(
                receivedObj.date,
                $"{receivedObj.fromHour}:{receivedObj.fromMinute}",
                $"{receivedObj.toHour}:{receivedObj.toMinute}",
                worker,
                receivedObj.breakAmount.ToString(),
                receivedObj.bossId.ToString()
            );

            workShift.ShiftId = receivedObj.shiftId;
 
            return workShift;


        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not edit shift");
        }
    }

    public Task DeleteAsync(int shiftId)
    {
        try
        {
            sender.RemoveShift(shiftId);
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not delete user");
        }
    }
}