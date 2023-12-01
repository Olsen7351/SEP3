using Broker.Services;
<<<<<<< HEAD
using Microsoft.AspNetCore.Authorization;
=======
using ClassLibrary_SEP3.DataTransferObjects;
>>>>>>> Test3
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
        return await _backlogService.AddTaskToBackLog(ProjectId,backLogTask);
    }

    [HttpDelete("{ProjectId}/Backlog/Task/{id}")]
    public async Task<IActionResult> DeleteTaskFromBacklog([FromRoute] string ProjectId, [FromRoute] string id)
    {
        return await _backlogService.DeleteTaskFromBacklog(id,ProjectId);
    }
}