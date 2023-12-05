using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Services;

namespace ProjectMicroservice.Controllers;



[Route("api/[controller]")]
[ApiController]

public class LogBookController : ControllerBase
{
    private readonly ILogBookService _iLogBookService;

    
    public LogBookController(ILogBookService iLogBookService)
    {
        _iLogBookService = iLogBookService;
    }
    
    
    
    
    //Get Log Books
    [HttpGet("GetEntriesForLogBook")]
    public async Task<IActionResult> GetLogbookForProject(String ProjectID)
    {
        if (String.IsNullOrEmpty(ProjectID))
        {
            return BadRequest("LogbookEntryPoint is required.");
        }
        try
        {
            var serviceResult =  _iLogBookService.GetLogbookForProject(ProjectID);
            return Ok(serviceResult);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }
    
    
    //Create Entries inside logbook
    [HttpPost("CreateLogEntryMicro")]
    public async Task<IActionResult> CreateLogBookEntry(LogBookEntryPoints logBookEntryPoints)
    {
        if (logBookEntryPoints == null)
        {
            return BadRequest("LogbookEntryPoint is required.");
        }
        try
        {
            var serviceResult =  _iLogBookService.CreateNewEntry(logBookEntryPoints);
            return Ok(serviceResult);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred");
        }
    }
}