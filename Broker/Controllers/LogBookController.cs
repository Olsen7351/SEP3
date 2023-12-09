using System.Runtime.InteropServices.JavaScript;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using ClassLibrary_SEP3.RabbitMQ;
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

    
    [HttpPut("UpdateEntry")]
    public async Task<IActionResult> UpdateEntry([FromBody] UpdateEntryRequest updateRequest)
    {
        var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
        if (updateRequest == null)
        {
            Logger.LogMessage(username+": Error updating Logbook entry: "+ updateRequest.ProjectID+ " | "+updateRequest.EntryID);
            return BadRequest("Request payload cannot be null");
        }
        
        if (String.IsNullOrEmpty(updateRequest.EntryID) || String.IsNullOrEmpty(updateRequest.ProjectID))
        {
            Logger.LogMessage(username+": Error updating Logbook entry: "+ updateRequest.ProjectID+ " | "+updateRequest.EntryID);
            return BadRequest("EntryID and ProjectID must not be null or empty.");
        }
        try
        {
            Logger.LogMessage(username+": Updating Logbook entry: "+ updateRequest.ProjectID+ " | "+updateRequest.EntryID);
            var result = await _iLogBookService.UpdateEntry(updateRequest);
            return Ok("Entry updated successfully.");
        }
        catch (Exception ex)
        {
            Logger.LogMessage(username+": Error updating Logbook entry: "+ updateRequest.ProjectID+ " | "+updateRequest.EntryID);
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }

    
    
    

    [HttpGet("{ProjectID}/logbookentries/{EntryID}")]
    public async Task<ActionResult<LogBookEntryPoints>> GetSpecificEntry(string ProjectID, string EntryID)
    {
        var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
        if (String.IsNullOrEmpty(EntryID) || String.IsNullOrEmpty(ProjectID))
        {
            Logger.LogMessage(username+": Error getting Logbook entry: "+ ProjectID+ " | "+EntryID);
            return BadRequest("EntryID and ProjectID must not be null or empty.");
        }

        try
        {
            var entry = await _iLogBookService.GetSpecificEntry(ProjectID, EntryID);
            if (entry != null)
            {
                Logger.LogMessage(username+": Getting Logbook entry: "+ ProjectID+ " | "+EntryID);
                return Ok(entry);
            }
            else
            {
                Logger.LogMessage(username+": Error getting Logbook entry: "+ ProjectID+ " | "+EntryID);
                return NotFound("LogBook entry not found.");
            }
        }
        catch (Exception ex)
        {
            Logger.LogMessage(username+": Error getting Logbook entry: "+ ProjectID+ " | "+EntryID);
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
        }
    }

    
    
    
    
    [HttpGet("GetLogEntries")]
    public async Task<ActionResult<LogBook>> GetLogbookForProject(string ProjectID)
    {
        var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
        if (String.IsNullOrEmpty(ProjectID))
        {
            Logger.LogMessage(username+": Error getting Logbook entries: "+ ProjectID);
            return BadRequest("ProjectID is required.");
        }
        try
        {
            Logger.LogMessage(username+": Getting Logbook entries: "+ ProjectID);
            var logBook = await _iLogBookService.GetEntriesForLogBook(ProjectID);
            return Ok(logBook);
        }
        catch (KeyNotFoundException knfe)
        {
            // Log the exception details here.
            Logger.LogMessage(username+": Error getting Logbook entries: "+ ProjectID);
            return NotFound(knfe.Message);
        }
        catch (HttpRequestException hre)
        {
            // Log the exception details here.
            Logger.LogMessage(username+": Error getting Logbook entries: "+ ProjectID);
            return StatusCode(StatusCodes.Status503ServiceUnavailable, hre.Message);
        }
        catch (Exception e)
        {
            // Log the exception details here.
            Logger.LogMessage(username+": Error getting Logbook entries: "+ ProjectID);
            return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {e.Message}");
        }
    }



    
    
    
    //Create New Entry
    [HttpPost("CreateLogEntryBroker")]
    public async Task<IActionResult> CreateLogBookEntry([FromBody] AddEntryPointRequest logBookEntryPoints)
    {
        var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);

        if (logBookEntryPoints == null)
        {
            throw new Exception("LogbookEntryPoint seems to be null?");
        }

        if (String.IsNullOrEmpty(logBookEntryPoints.ProjectID))
        {
            throw new Exception("ProjectID cant be null or empty when creating a new entry");
        }

        if (String.IsNullOrEmpty(logBookEntryPoints.OwnerUsername))
        {
            throw new Exception("Username cant be null or empty when creating a new entry");
        }

        var serviceResult = await _iLogBookService.CreateNewEntryLogBook(logBookEntryPoints);
        return serviceResult;
    }
}