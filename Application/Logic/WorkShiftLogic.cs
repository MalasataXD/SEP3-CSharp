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
            
        return await _WorkShiftDao.CreateAsync(workShift);
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
        updatedWorkShift.ShiftId = workShift.ShiftId;
        
         await ValidateWorkShift(updatedWorkShift, updatedWorkShift.ShiftId);

        await _WorkShiftDao.UpdateAsync(updatedWorkShift);
    }

    public async Task DeleteAsync(int shiftId)
    {
        /*
        WorkShift? workShift = await _WorkShiftDao.GetByIdAsync(shiftId);
        if (workShift == null)
        {
            throw new Exception("Workshift does not exist!");
        }
        */

        await _WorkShiftDao.DeleteAsync(shiftId);
    }


    private async Task ValidateWorkShift(WorkShift workShift, int? workShiftId = 0)
    {
        //todo check for absence
        //todo add unitTesting!
        ValidateDate(workShift.Date);
        ValidateTime(workShift.FromTime); 
        ValidateTime(workShift.ToTime);
        ValidateTimes(workShift.FromTime, workShift.ToTime);
        IEnumerable<WorkShift> shifts = await _WorkShiftDao.GetAsync(new SearchShiftParametersDto(workShift.Date,null));

        if (workShiftId != 0)
        {
            shifts = shifts.Where(shift => shift.ShiftId != workShiftId).ToList();
        }
        
        IsWorkerOccupied(workShift.Worker.WorkerId, workShift.Date, workShift.FromTime, workShift.ToTime,shifts);
    }

    private void ValidateDate(string date)
    {
        string[] temp = date.Split("/");
        
        // todo add methods: how many days in month + isLeapYear + more??
        // ? prob use DateTime class ?
    }

    private void ValidateTime(string time)
    {

        try
        {
            string[] timeSplit = time.Split(":", 2);
        
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
        catch (Exception e)
        {
            throw new Exception("Invalid time format");
        }
        

    }

    private void ValidateTimes(string fromTime, string toTime)
    {
        if (TimeToMinutes(fromTime) >= TimeToMinutes(toTime))
        {
            throw new Exception($"{fromTime} {toTime} fromTime has to be before toTime");
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

    private int TimeToMinutes(string time)
    {
        string[] temp = time.Split(":");

        return Int32.Parse(temp[0]) * 60 + Int32.Parse(temp[1]);
    }
}