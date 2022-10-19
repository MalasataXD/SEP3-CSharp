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

        WorkShift workShift = new(toCreate.Date, toCreate.ToTime, toCreate.FromTime, worker, toCreate.BreakAmount);

        ValidateWorkShift(workShift);
     
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


    private void ValidateWorkShift(WorkShift workShift)
    {
        //todo check for absence
        //todo add unitTesting!
        ValidateDate(workShift.Date);
        ValidateTime(workShift.FromTime);
        ValidateTime(workShift.ToTime);
        ValidateTimes(workShift.FromTime, workShift.ToTime);
        IsWorkerOccupied(workShift.Worker.getFullName(), workShift.Date, workShift.FromTime, workShift.ToTime);
    }

    private void ValidateDate(string date)
    {
        string[] temp = date.Split("/");
        
        // todo add methods: how many days in month + isLeapYear + more??
        // ? make date class ?
    }

    private void ValidateTime(string time)
    {
        try
        {
            char[] temp = time.ToCharArray();
        
            int timeHour = temp[0] * 10 + temp[1];
            int timeMinute = temp[3] * 10 + temp[4];

            if (!(temp[2].ToString().Equals(":") && time.Length == 5))
            {
                throw new Exception("Invalid timeformat");
            }
            
            if (!(timeHour is >= 0 and <= 23))
            {
                throw new Exception("Invalid time");
            }

            if (!(timeMinute is >= 0 and <= 59))
            {
                throw new Exception("Invalid time");
            }
        }
        catch (Exception e)
        {
            throw new Exception("Invalid timeformat");
        }
    }

    private void ValidateTimes(string fromTime, string toTime)
    {
        if (TimeToMinutes(fromTime) >= TimeToMinutes(toTime))
        {
            throw new Exception("fromTime has to be before toTime");
        }
    }

    private async void IsWorkerOccupied(string fullName, string date, string fromTime, string toTime)
    {
        SearchShiftParametersDto dto = new (date, fullName);
        IEnumerable<WorkShift> workShifts = await _WorkShiftDao.GetAsync(dto);

        int fromTimeMinutes = TimeToMinutes(fromTime);
        int toTimeMinutes = TimeToMinutes(toTime);
        
        foreach (var workShift in workShifts)
        {
            if (fromTimeMinutes <= TimeToMinutes(workShift.FromTime) && TimeToMinutes(workShift.FromTime) <= toTimeMinutes)
            {
                throw new Exception($"{fullName} already has a shift at that time!");
            }

            if (fromTimeMinutes <= TimeToMinutes(workShift.ToTime) && TimeToMinutes(workShift.ToTime) <= toTimeMinutes)
            {
                throw new Exception($"{fullName} already has a shift at that time!");
            }

            if (TimeToMinutes(workShift.FromTime) <= fromTimeMinutes && toTimeMinutes <= TimeToMinutes(workShift.ToTime))
            {
                throw new Exception($"{fullName} already has a shift at that time!");
            }
        }
    }

    private int TimeToMinutes(string time)
    {
        char[] temp = time.ToCharArray();

        return temp[0] * 600 + temp[1] * 60 + temp[3] * 10 + temp[4];
    }
}