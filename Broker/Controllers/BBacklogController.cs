using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[Route("api/[controller]")]
public class BBacklogController : ControllerBase
{
    //Field 
    private readonly IBBacklogService _iBBacklogService;

    public BBacklogController(IBBacklogService iBBacklogService)
    {
        _iBBacklogService = iBBacklogService;
    }


    [HttpPost]
    [Route("CreateBroker")]
    public async Task<IActionResult> Create([FromBody] AddBacklogEntryRequest backlogEntry)
    {
        if (backlogEntry == null)
        {
            return BadRequest("Backlog entry payload is null");
        }

        if (String.IsNullOrEmpty(backlogEntry.ProjectID))
        {
            throw new Exception("projectID is null or empty broker");
        }

        try
        {
            var result = await _iBBacklogService.CreateBacklogEntry(backlogEntry);
            return Ok("Entry pushed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }


    [HttpGet]
    [Route("GetBacklogBroker")]
    public async Task<IActionResult> GetBacklogForProject(string projectID)
    {
        if (string.IsNullOrEmpty(projectID))
        {
            return BadRequest("ProjectID can't be null or empty");
        }

        try
        {
            var result = await _iBBacklogService.GetBacklogForProject(projectID);
            return Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Consider logging the exception to a log file or logging system here
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}