using Application.LogicInterfaces;
using Domain.DTOs.SearchParameters;
using Domain.DTOs.WorkShift;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WorkPlannerAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class WorkShiftController : ControllerBase
{
    // * Fields
    private readonly IWorkShiftLogic _workShiftLogic;

    // ¤ constructor
    public WorkShiftController(IWorkShiftLogic workShiftLogic)
    {
        _workShiftLogic = workShiftLogic;
    }
    
    // ¤ Create WorkShift
    [HttpPost]
    public async Task<ActionResult<WorkShift>> CreateAsync(WorkShiftCreationDto toCreate)
    {
        try
        {
            WorkShift created = await _workShiftLogic.CreateAsync(toCreate);
            return new OkObjectResult(created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // ¤ Get WorkShift
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkShift>>> GetAsync(string? date, string? workerName)
    {
        try
        {
            SearchShiftParametersDto parameters = new(date, workerName);
            IEnumerable<WorkShift> workShifts = await _workShiftLogic.GetAsync(parameters);
            return new OkObjectResult(workShifts);
        }
        catch (Exception e)
        {
            Console.WriteLine("Could not find any user matching the criteria ");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    // ¤ Update WorkShift
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync([FromBody] WorkShiftUpdateDto toUpdate)
    {
        try
        {
            await _workShiftLogic.UpdateAsync(toUpdate);
            return new OkResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    // ¤ Delete WorkShift
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _workShiftLogic.DeleteAsync(id);
            return new OkResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }





}