using System.Runtime.InteropServices.JavaScript;
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


    [HttpGet("{ProjectID}/logbookentries/{EntryID}")]
    public async Task<ActionResult<LogBookEntryPoints>> GetSpecificEntry(string ProjectID, string EntryID)
    {
        if (String.IsNullOrEmpty(EntryID) || String.IsNullOrEmpty(ProjectID))
        {
            return BadRequest("EntryID and ProjectID must not be null or empty.");
        }

        try
        {
            var entry = await _iLogBookService.GetSpecificEntry(ProjectID, EntryID);
            if (entry != null)
            {
                return Ok(entry);
            }
            else
            {
                return NotFound("LogBook entry not found.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }

    
    
    
    
    [HttpGet("GetLogEntries")]
    public async Task<ActionResult<LogBook>> GetLogbookForProject(string ProjectID)
    {
        if (String.IsNullOrEmpty(ProjectID))
        {
            return BadRequest("ProjectID is required.");
        }
        try
        {
            var logBook = await _iLogBookService.GetEntriesForLogBook(ProjectID);
            return Ok(logBook);
        }
        catch (KeyNotFoundException knfe)
        {
            return NotFound(knfe.Message);
        }
        catch (HttpRequestException hre)
        {
            // Log the exception details here.
            return StatusCode(StatusCodes.Status503ServiceUnavailable, hre.Message);
        }
        catch (Exception e)
        {
            // Log the exception details here.
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {e.Message}");
        }
    }



    
    
    
    //Create New Entry
    [HttpPost("CreateLogEntryBroker")]
    public async Task<IActionResult> CreateLogBookEntry([FromBody] AddEntryPointRequest logBookEntryPoints)
    {
        if (logBookEntryPoints == null)
        {
            throw new Exception("LogbookEntryPoint seems to be null?");
        }

        var serviceResult = await _iLogBookService.CreateNewEntryLogBook(logBookEntryPoints);
        return serviceResult;
    }
}