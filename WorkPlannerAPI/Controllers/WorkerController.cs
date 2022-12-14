using Application.LogicInterfaces;
using Domain.DTOs.SearchParameters;
using Domain.DTOs.Worker;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WorkPlannerAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class WorkerController : ControllerBase
{
    // # Fields
    private readonly IWorkerLogic _workerLogic;
    
    // ¤ Constructor
    public WorkerController(IWorkerLogic workerLogic)
    {
        _workerLogic = workerLogic;
    }
    
    // ¤ Create Worker
    [HttpPost]
    public async Task<ActionResult<Worker>> CreateAsync(WorkerCreationDto toCreate)
    {
        try
        {
            Worker created = await _workerLogic.CreateAsync(toCreate);
            return new OkObjectResult(created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // ¤ Get Worker
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Worker>>> GetAsync([FromQuery] string? workerName)
    {
        try
        {
            SearchWorkerParametersDto parameters = new(workerName);
            IEnumerable<Worker> workers = await _workerLogic.GetAsync(parameters);
            return new OkObjectResult(workers);
        }
        catch (Exception e)
        {
            Console.WriteLine("Could not find any user matching the criteria ");
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    
    // ¤ Update Worker
    [HttpPatch]
    public async Task<ActionResult> UpdateAsync([FromBody] WorkerUpdateDto toUpdate)
    {
        try
        {
            await _workerLogic.UpdateAsync(toUpdate);
            return new OkResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    // ¤ Delete Worker
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            await _workerLogic.DeleteAsync(id);
            return new OkResult();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
    
    
    
}