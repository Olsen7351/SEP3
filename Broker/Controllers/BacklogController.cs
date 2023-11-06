using Broker.Services;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> AddTaskToBacklog([FromRoute] int ProjectId, [FromBody] Task backLogTask)
    {
        if (backLogTask == null)
        {
            return BadRequest();
        }
        return Ok(await _backlogService.AddTaskToBackLog(ProjectId,backLogTask));
    }

    [HttpDelete("{ProjectId}/Backlog/Delete")]
    public async Task<IActionResult> DeleteTaskFromBacklog([FromRoute] int ProjectId,
        [FromBody] DeleteBacklogTaskRequest backlogTask)
    {
        if (backlogTask == null)
        {
            return BadRequest();
        }

        return Ok(await _backlogService.DeleteTaskFromBacklog(ProjectId, IdBacklog,backlogTask));
    }
}