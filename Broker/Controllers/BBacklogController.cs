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
    [Route("GetBacklogBroker/{projectID}")] 
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
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }


    [HttpGet("{projectId}/backlogentries/{backlogEntryId}")]
    public async Task<ActionResult<BacklogEntries>> GetSpecificBacklogEntry(string projectId, string backlogEntryId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            return BadRequest("ProjectID can't be null or empty");
        }

        if (string.IsNullOrEmpty(backlogEntryId))
        {
            return BadRequest("BacklogEntryID can't be null or empty");
        }

        try
        {
            var backlogEntry = await _iBBacklogService.GetSpecificBacklogEntry(projectId, backlogEntryId);
            if (backlogEntry != null)
            {
                return Ok(backlogEntry);
            }
            else
            {
                return NotFound("Backlog entry not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPut]
    [Route("UpdateEntry")]
    public async Task<IActionResult> UpdateBacklogEntry([FromBody] UpdateBacklogEntryRequest updateBacklogEntryRequest)
    {
        if (String.IsNullOrEmpty(updateBacklogEntryRequest.EntryID))
        {
            return BadRequest("EntryID is null or empty Broker");
        }

        if (String.IsNullOrEmpty(updateBacklogEntryRequest.ProjectID))
        {
            return BadRequest("ProjectID is null or empty Broker");
        }

        try
        {
            var result = await _iBBacklogService.UpdateBacklogEntry(updateBacklogEntryRequest);
            if (result)
            {
                return Ok("Backlog entry updated successfully.");
            }
            else
            {
                return NotFound("Backlog entry not found.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {e.Message}");
        }
    }
}