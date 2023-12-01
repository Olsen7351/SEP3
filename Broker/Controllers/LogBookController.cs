using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;


namespace Broker.Controllers;

[ApiController]
[Route("api/[controller]")]

public class LogBookController : ControllerBase
{
    
    //Field 
    private readonly ILogBookService _iLogBookService;

    
    //Constructor
    public LogBookController(ILogBookService iLogBookService)
    {
        _iLogBookService = iLogBookService;
    }

    // Get Logbook for project
    [HttpGet("GetLogEntries")]
    public async Task<IActionResult> GetLogbook([FromQuery] string projectID)
    {
        if (string.IsNullOrEmpty(projectID))
        {
            return BadRequest("ProjectID is required.");
        }

        try
        {
            var entries = await _iLogBookService.GetEntriesForLogBook(projectID);
            return Ok(entries);
        }
        
        catch (Exception e)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    
    
    
    //Create New Entry
    [HttpPost("CreateLogEntry")]
    public async Task<IActionResult> CreateLogBookEntry([FromBody] LogBookEntryPoints logBookEntryPoints)
    {
        if (logBookEntryPoints == null)
        {
            throw new Exception("LogbookEntryPoint seems to be null?");
        }

        var serviceResult = await _iLogBookService.CreateNewEntryLogBook(logBookEntryPoints);
        return serviceResult;
    }
}