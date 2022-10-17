using Application.DAOInterfaces;
using Domain.DTOs.SearchParameters;
using Domain.Models;

namespace FileData.DAOs;

public class WorkShiftFileDao : IWorkShiftDao
{
    // # Fields
    private readonly FileContext _context;
    
    // ¤ Constructor
    public WorkShiftFileDao(FileContext context)
    {
        _context = context;
    }
    
    // ¤ Create new Shift
    public Task<WorkShift> CreateAsync(WorkShift shift)
    {
        int shiftId = 1;
        
        // * Check if there exists more shifts
        if (_context.Shifts.Any())
        {
            shiftId = _context.Shifts.Max(s => s.ShiftId);
            shiftId++;
        }

        // * Assign Shift the correct Id
        shift.ShiftId = shiftId;
        
        // * Save the new shift in the file (temp)
        _context.Shifts.Add(shift);
        _context.SaveChanges();

        return Task.FromResult(shift);
    }

    // ¤ Get Shift (Search Parameters)
    public Task<IEnumerable<WorkShift>> GetAsync(SearchShiftParametersDto searchParameters)
    {
        IEnumerable<WorkShift> result = _context.Shifts.AsEnumerable();

        if (!string.IsNullOrEmpty(searchParameters.Date))
        {
            result = _context.Shifts.Where(shift =>
                shift.Date.Equals(searchParameters.Date, StringComparison.OrdinalIgnoreCase));
        }
        
        if (!string.IsNullOrEmpty(searchParameters.WorkerName))
        {
            result = _context.Shifts.Where(shift =>
                shift.Date.Equals(searchParameters.Date, StringComparison.OrdinalIgnoreCase));
        }
        return Task.FromResult(result);
    }

    // ¤ Get by Id
    public Task<WorkShift?> GetByIdAsync(int shiftId)
    {
        WorkShift? existing = _context.Shifts.FirstOrDefault(s => s.ShiftId == shiftId);
        return Task.FromResult(existing);
    }

    // ¤ Update Shift
    public Task UpdateAsync(WorkShift toUpdate)
    {
        // * Check if a shift with the id exists
        WorkShift? existing = _context.Shifts.FirstOrDefault(shift => shift.ShiftId == toUpdate.ShiftId);
        if (existing == null)
        {
            throw new Exception($"A Shift with id {toUpdate.ShiftId} does not exist!");
        }

        // * Replace the existing shift with the Update
        _context.Shifts.Remove(existing);
        _context.Shifts.Add(toUpdate);
        
        // * Save changes
        _context.SaveChanges();

        return Task.CompletedTask;
    }

    // ¤ Delete Shift
    public Task DeleteAsync(int shiftId)
    {
        // * Check if a shift with the id exists
        WorkShift? existing = _context.Shifts.FirstOrDefault(shift => shift.ShiftId == shiftId);
        if (existing == null)
        {
            throw new Exception($"A Shift with id {shiftId} does not exist!");
        }
        
        // * Remove the existing
        _context.Shifts.Remove(existing);
        
        // * Save changes
        _context.SaveChanges();

        return Task.CompletedTask;
    }
}