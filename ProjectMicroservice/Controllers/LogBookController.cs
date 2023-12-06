using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
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
    
    
    
    [HttpGet("GetEntriesForLogBook")]
    public async Task<ActionResult<LogBook>> GetLogbookForProject(string ProjectID)
    {
        if (String.IsNullOrEmpty(ProjectID))
        {
            return BadRequest("ProjectID is required.");
        }
        try
        {
            var serviceResult = await _iLogBookService.GetLogbookForProject(ProjectID); 
            if (serviceResult == null)
            {
                return NotFound($"Logbook for project ID {ProjectID} not found.");
            }
            return Ok(serviceResult);
        }
        catch (KeyNotFoundException knfe)
        {
            return NotFound(knfe.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {e.Message}");
        }
        //Return logbook?
    }

    
    
    //Create Entries inside logbook
    [HttpPost("CreateLogEntryMicro")]
    public async Task<IActionResult> CreateLogBookEntry(AddEntryPointRequest logBookEntryPoints)
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
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
}