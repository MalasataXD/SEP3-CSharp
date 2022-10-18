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
            Console.WriteLine("Could not create new user, something went wrong!");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
    
}