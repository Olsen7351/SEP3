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
    
    
    
    [HttpPut("UpdateEntry")]
    public async Task<IActionResult> UpdateLogBookEntry([FromBody] UpdateEntryRequest updateEntryRequest)
    {
        if (updateEntryRequest == null)
        {
            return BadRequest("Update request must not be null.");
        }

        if (String.IsNullOrEmpty(updateEntryRequest.ProjectID) || String.IsNullOrEmpty(updateEntryRequest.EntryID))
        {
            return BadRequest("ProjectID and EntryID must not be null or empty.");
        }

        try
        {
            var result = await _iLogBookService.UpdateLogBookEntry(updateEntryRequest);
            if (result)
            {
                return Ok("Entry updated successfully.");
            }
            else
            {
                return NotFound("Entry not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }
    
    
    [HttpGet("{ProjectID}/logbookentries/{EntryID}")]
    public async Task<ActionResult<LogBookEntryPoints>> GetSpecificLogBookEntry(string ProjectID, string EntryID)
    {
        if (String.IsNullOrEmpty(ProjectID))
        {
            return BadRequest("ProjectID is required.");
        }

        if (String.IsNullOrEmpty(EntryID))
        {
            return BadRequest("EntryID is required.");
        }

        try
        {
            var logBookEntry = await _iLogBookService.GetSpecificLogBookEntry(ProjectID, EntryID); 
            if (logBookEntry == null)
            {
                return NotFound($"Logbook entry with ID {EntryID} for project ID {ProjectID} not found.");
            }
            return Ok(logBookEntry);
        }
        catch (KeyNotFoundException knfe)
        {
            return NotFound(knfe.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {e.Message}");
        }
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