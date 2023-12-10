using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Services;
using Task = ClassLibrary_SEP3.Task;

namespace ProjectMicroservice.Controllers;

[Route("api/[controller]")]
public class BBacklogController : ControllerBase
{
    //Field 
    private readonly IBacklogService _backlogService;

    
    public BBacklogController(IBacklogService iBacklogService)
    {
        _backlogService = iBacklogService;
    }

    
    
    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] AddBacklogEntryRequest backlogEntry)
    {
        if (backlogEntry == null)
        {
            return BadRequest("Backlog entry payload is null");
        }
        
        if (String.IsNullOrEmpty(backlogEntry.ProjectID))
        {
            throw new Exception("ProjectID is null or empty MircoService");
        }

        try
        {
            var result = await _backlogService.CreateBacklogEntry(backlogEntry);
            return Ok("Entry pushed successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }



    [HttpGet]
    [Route("GetBacklogMicro/{projectID}")]
    public async Task<BBackLog> GetBacklogForProject(string projectID)
    {
        if (string.IsNullOrEmpty(projectID))
        {
            throw new ArgumentException("ProjectID is null or empty");
        }

        try
        {
            return await _backlogService.GetBacklogForProject(projectID);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // Log the exception here
            throw new InvalidOperationException("An error occurred while processing your request.");
        }
    }
}