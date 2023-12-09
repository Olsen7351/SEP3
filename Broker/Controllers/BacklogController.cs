using Broker.Services;

using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;

using ClassLibrary_SEP3.DataTransferObjects;
using ClassLibrary_SEP3.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[Route("api/")]
[Authorize]
public class BacklogController : ControllerBase
{
    private readonly IBacklogService _backlogService;

    public BacklogController(IBacklogService backlogService)
    {
        _backlogService = backlogService;
    }
    
    //AddTaskToBacklog
    [HttpPost("{ProjectId}/Backlog/AddTask")]
    public async Task<Task> AddTaskToBacklog([FromRoute] string ProjectId, [FromBody] AddBacklogTaskRequest? backLogTask)
    {
        var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
        try
        {
            Logger.LogMessage(username + ": Adding task to backlog: " + backLogTask);
            return await _backlogService.AddTaskToBackLog(ProjectId, backLogTask);
        }
        catch (Exception e)
        {
            Logger.LogMessage(username+": Failed to add task to backlog: "+e.Message);
            throw;
        }
        
    }

    [HttpDelete("{ProjectId}/Backlog/Task/{id}")]
    public async Task<IActionResult> DeleteTaskFromBacklog([FromRoute] string ProjectId, [FromRoute] string id)
    {
        var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
        try
        {
            Logger.LogMessage(username + ": Deleting task from backlog: " + id);
            return await _backlogService.DeleteTaskFromBacklog(id, ProjectId);
        }
        catch (Exception e)
        {
            Logger.LogMessage(username+": Failed to delete task from backlog: "+e.Message);
            return BadRequest(e.Message);
        }
        
    }
}