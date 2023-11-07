using Broker.Services;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[Route("api/")]
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
    public async Task<IActionResult> DeleteTaskFromBacklog([FromRoute] string id, string ProjectId)
    {
        return await _backlogService.DeleteTaskFromBacklog(ProjectId,id);
    }
}