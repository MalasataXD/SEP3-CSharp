﻿using Domain.DTOs.JavaDTOs;
using Domain.DTOs.SearchParameters;
using Domain.Models;

namespace Application.DAOInterfaces;

public interface IWorkShiftDao
{
    Task<WorkShift> CreateAsync(WorkShift shift); // ¤ Create (Post)
    Task<IEnumerable<WorkShift>> GetAsync(SearchShiftParametersDto searchParameters); // ¤ Get
    Task<WorkShift?> GetByIdAsync(int shiftId); // ¤ Get
    Task<WorkShift> UpdateAsync(WorkShift toUpdate); // ¤ Update (Patch)
    Task DeleteAsync(int shiftId); // ¤ Delete
    
    Task<bool> DeleteAsync(List<int> shiftIds); // ¤ Delete
    Task<bool> CreateAsync(List<WorkShift> shifts); // ¤ Delete
    Task<bool> UpdateAsync(List<WorkShift> toUpdate); // ¤ Update (Patch)
}