using Application.DAOInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.SearchParameters;
using Domain.DTOs.Worker;
using Domain.DTOs.WorkShift;
using Domain.Models;

namespace Application.Logic;

public class WorkShiftLogic : IWorkShiftLogic
{
    
    private readonly IWorkShiftDao _WorkShiftDao;
    private readonly IWorkerDao _WorkerDao;

    public WorkShiftLogic(IWorkShiftDao workShiftDao, IWorkerDao workerDao)
    {
        _WorkShiftDao = workShiftDao;
        _WorkerDao = workerDao;
    }

    public async Task<WorkShift> CreateAsync(WorkShiftCreationDto toCreate)
    {
        Worker? worker = await _WorkerDao.GetByIdAsync(toCreate.WorkerId);
        if (worker == null)
        {
            throw new Exception("Worker does not exist!");
        }

        WorkShift workShift = new(toCreate.Date, toCreate.FromTime, toCreate.ToTime, worker, toCreate.BreakAmount);

        await ValidateWorkShift(workShift);
        
        WorkShift created = await _WorkShiftDao.CreateAsync(workShift);

        return created;
    }

    public async Task<IEnumerable<WorkShift>> GetAsync(SearchShiftParametersDto searchParameters)
    {
        return await _WorkShiftDao.GetAsync(searchParameters);
    }

    // NOTE : Could be removed if not needed
    public async Task<WorkShift?> GetByIdAsync(int shiftId)
    {
        return await _WorkShiftDao.GetByIdAsync(shiftId);
    }

    public async Task UpdateAsync(WorkShiftUpdateDto toUpdate)
    {

        WorkShift? workShift = await _WorkShiftDao.GetByIdAsync(toUpdate.ShiftId);

        if (workShift == null)
        {
            throw new Exception("Workshift does not exist!");
        }

        Worker? worker = null;
        if (toUpdate.WorkerId != null)
        {
             worker = await _WorkerDao.GetByIdAsync((int) toUpdate.WorkerId);

            if (worker == null)
            {
                throw new Exception("Worker does not exist!");
            }
        }

        Worker workerToUse = worker ?? workShift.Worker;
        string dateToUse = toUpdate.Date ?? workShift.Date;
        string fromTimeToUse = toUpdate.FromTime ?? workShift.FromTime;
        string toTimeToUse = toUpdate.ToTime ?? workShift.ToTime;
        string breakAmountToUse = toUpdate.BreakAmount ?? workShift.BreakAmount;

        WorkShift updatedWorkShift = new(dateToUse, fromTimeToUse, toTimeToUse, workerToUse, breakAmountToUse);
        await _WorkShiftDao.UpdateAsync(updatedWorkShift);
    }

    public async Task DeleteAsync(int shiftId)
    {
        WorkShift? workShift = await _WorkShiftDao.GetByIdAsync(shiftId);
        if (workShift == null)
        {
            throw new Exception("Workshift does not exist!");
        }

        await _WorkShiftDao.DeleteAsync(shiftId);
    }


    private async Task ValidateWorkShift(WorkShift workShift)
    {
        ValidateDate(workShift.Date);
        ValidateTime(workShift.FromTime);
        ValidateTime(workShift.ToTime);
        ValidateTimes(workShift.FromTime, workShift.ToTime);
        IEnumerable<WorkShift> shifts = await _WorkShiftDao.GetAsync(new SearchShiftParametersDto(workShift.Date,null));
        IsWorkerOccupied(workShift.Worker.WorkerId, workShift.Date, workShift.FromTime, workShift.ToTime,shifts);
    }

    private void ValidateDate(string date)
    {
        string[] temp = date.Split("/");
        
        // todo add methods: how many days in month + isLeapYear + more??
        // ? make date class ?
    }

    private void ValidateTime(string time)
    {
        string[] timeSplit = time.Split(":", 2);
        
        Console.WriteLine(timeSplit[0]);
        Console.WriteLine(timeSplit[1]);
        
            if (timeSplit[0].Length != 2 && timeSplit[1].Length != 2)
            {
                throw new Exception("Invalid time format");
            }

            if (Int32.Parse(timeSplit[0].TrimStart('0')) is < 0 and <= 23)
            {
                throw new Exception("Invalid hour");
            }
            
            if (Int32.Parse(timeSplit[1]) is < 0 and <= 59)
            {
                throw new Exception("Invalid minute");
            }
        
    }

    private void ValidateTimes(string fromTime, string toTime)
    {
        string[] fromTimeSplit = fromTime.Split(":", 2);
        string[] toTimeSplit = toTime.Split(":", 2);
        
        if(Int32.Parse(toTimeSplit[0]) < Int32.Parse(fromTimeSplit[0]))
        {
            throw new Exception("toTime is before fromTime!");
        }
        else if (Int32.Parse(toTimeSplit[0]) == Int32.Parse(fromTimeSplit[0]))
        {
            if(Int32.Parse(toTimeSplit[1]) <= Int32.Parse(fromTimeSplit[1]))
            {
                throw new Exception("toTime is not after fromTime!");
            }
        }
    }

    private void IsWorkerOccupied(int id, string date, string fromTime, string toTime,IEnumerable<WorkShift>  shifts)
    {
        SearchShiftParametersDto dto = new (date,null);
        IEnumerable<WorkShift> workShifts = shifts;
        int fromTimeMinutes = TimeToMinutes(fromTime);
        int toTimeMinutes = TimeToMinutes(toTime);
        Boolean throwExeception = false;
        WorkShift foundShift = null;
        
        
        foreach (var workShift in workShifts)
        {
            if (id == workShift.Worker.WorkerId)
            {
                if (fromTimeMinutes <= TimeToMinutes(workShift.FromTime) && TimeToMinutes(workShift.FromTime) <= toTimeMinutes)
                {
                    throwExeception = true;
                    foundShift = workShift;
                    break;
                }

                if (fromTimeMinutes <= TimeToMinutes(workShift.ToTime) && TimeToMinutes(workShift.ToTime) <= toTimeMinutes)
                {
                    throwExeception = true;
                    foundShift = workShift;
                    break;
                }

                if (TimeToMinutes(workShift.FromTime) <= fromTimeMinutes && toTimeMinutes <= TimeToMinutes(workShift.ToTime))
                {
                    throwExeception = true;
                    foundShift = workShift;
                    break;
                }
            }
        }

        if (throwExeception)
        {
            throw new Exception($"{foundShift.Worker.getFullName()} already has a shift at that time!");
        }
        
    }

    // ¤ Time (string to ints)
    private static int[] TimeToInts(string time)
    {


        return new int[] { 0 };
    }
    
    
    private int TimeToMinutes(string time)
    {
        char[] temp = time.ToCharArray();

        return temp[0] * 600 + temp[1] * 60 + temp[3] * 10 + temp[4];
    }
}