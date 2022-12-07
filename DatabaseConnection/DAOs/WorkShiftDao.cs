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
    private readonly IWorkerDao workerDao;

    public WorkShiftDao(Sender sender, Receiver receiver, IWorkerDao workerDao)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.workerDao = workerDao;
    }

    public async Task<WorkShift> CreateAsync(WorkShift shift)
    {
        try
        {
            // # Send "CreateShift" to dispatcher
            sender.CreateShift(shift);
            
            // < Wait until we have received the shift
            object obj = await receiver.Receive("CreateShift");
            ShiftJavaDto? dto = JsonSerializer.Deserialize<ShiftJavaDto>((JsonElement) obj);
            
            // # Set Worker Id in shift
            Worker worker = new Worker("", "", 0, "", "");
            worker.WorkerId = dto.workerId;
            
            
            // # Return the shift.
            return new WorkShift(dto.shiftId,dto.date, dto.fromHour + ":" + dto.fromMinute, dto.toHour + ":" + dto.toMinute, worker, dto.breakAmount.ToString(), dto.bossId.ToString());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not create shift");
        }
    }

    public async Task<IEnumerable<WorkShift>> GetAsync(SearchShiftParametersDto searchParameters)
    {
        try
        {
            sender.GetShiftBySearchParameters(new SearchShiftParametersJavaDto(searchParameters));

            object obj = await receiver.Receive("GetShiftBySearchParameters");

            List<ShiftJavaDto>? dto = JsonSerializer.Deserialize<List<ShiftJavaDto>>((JsonElement)obj);
            
            List<WorkShift> result = new List<WorkShift>();

            int oldItemWorkerId = 0;
            Worker worker = new Worker("","",0,"","");
            foreach (var item in dto)
            {
                
                Console.WriteLine(oldItemWorkerId + " != " + item.workerId);
                if (oldItemWorkerId != item.workerId)
                {
                    worker = await workerDao.GetByIdAsync(item.workerId);
                }
                oldItemWorkerId = item.workerId;

                WorkShift workShift = new WorkShift(
                    item.date,
                    $"{item.fromHour}:{item.fromMinute}",
                    $"{item.toHour}:{item.toMinute}",
                    worker,
                    item.breakAmount.ToString(),
                    item.bossId.ToString()
                );
                workShift.ShiftId = item.shiftId;
                
                result.Add(workShift);
            }
            return result.AsEnumerable();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not get worker");
        }
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
            Console.WriteLine("toUpdate shift : " + toUpdate.BreakAmount);
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

    public async Task<bool> DeleteAsync(List<int> shiftIds)
    {
        try
        {
            sender.DeleteAsync(shiftIds);
            
            object obj = await receiver.Receive("RemoveShifts");
            bool? receivedObj = JsonSerializer.Deserialize<bool>((JsonElement)obj);
            
            return (bool)receivedObj;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not delete users");
        }
    }

    public async Task<bool> CreateAsync(List<WorkShift> shifts)
    {
        try
        {
            sender.CreateAsync(shifts);
            
            object obj = await receiver.Receive("CreateShifts");
            bool? receivedObj = JsonSerializer.Deserialize<bool>((JsonElement)obj);
            
            return (bool)receivedObj;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not create users");
        }
    }

    public async Task<bool> UpdateAsync(List<WorkShift> toUpdate)
    {
        try
        {
            sender.CreateAsync(toUpdate);
            
            object obj = await receiver.Receive("EditShifts");
            bool? receivedObj = JsonSerializer.Deserialize<bool>((JsonElement)obj);
            
            return (bool)receivedObj;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Could not update users");
        }
    }
}