using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

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
}