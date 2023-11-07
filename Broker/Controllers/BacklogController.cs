using Broker.Services;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[Route("api/[controller]")]
public class BacklogController : ControllerBase
{
    private readonly IBacklogService _backlogService;

    public BacklogController(IBacklogService backlogService)
    {
        _backlogService = backlogService;
    }
    
    //AddTaskToBacklog
    [HttpPost("{ProjectId}/Backlog/Add")]
    public async Task<Task> AddTaskToBacklog([FromRoute] string ProjectId, [FromBody] AddBacklogTaskRequest? backLogTask)
    {
        return await _backlogService.AddTaskToBackLog(ProjectId,backLogTask);
    }

    [HttpDelete("{ProjectId}/Backlog/Delete")]
    public async Task<IActionResult> DeleteTaskFromBacklog([FromRoute] string ProjectId,
        [FromBody] DeleteBacklogTaskRequest backlogTask)
    {
        return await _backlogService.DeleteTaskFromBacklog(ProjectId,backlogTask);
    }
}