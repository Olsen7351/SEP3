
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Services;

namespace ProjectMicroservice.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SprintController : ControllerBase
{
    private readonly ISprintService _sprintService;

    public SprintController(ISprintService sprintService)
    {
        _sprintService = sprintService;
    }

    [HttpGet("{projectId}/backlog")]
    public async Task<IActionResult> GetSprintBacklog(string projectId)
    {
        // Implementation for getting the sprint backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpGet("{projectId}/backlog/sprints")]
    public async Task<IActionResult> GetAllSprints(string projectId)
    {
        // Implementation for getting all sprints in the backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpGet("{projectId}/backlog/sprints/{sprintId}")]
    public async Task<IActionResult> GetSpecificSprint(string projectId, string sprintId)
    {
        // Implementation for getting a specific sprint in the backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpPost("{projectId}/backlog/sprints")]
    public async Task<IActionResult> CreateSprint(string projectId, [FromBody] CreateSprintBackLogRequest  request)
    {
        // Implementation for creating a new sprint in the backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpPut("{projectId}/backlog/sprints/{sprintId}")]
    public async Task<IActionResult> UpdateSprint(string projectId, string sprintId, [FromBody] SprintBacklog sprint)
    {
        // Implementation for updating a specific sprint in the backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpDelete("{projectId}/backlog/sprints/{sprintId}")]
    public async Task<IActionResult> DeleteSprint(string projectId, string sprintId)
    {
        // Implementation for deleting a specific sprint in the backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpPost("{projectId}/backlog/sprints/{sprintId}/tasks")]
    public async Task<IActionResult> AddTaskToSprint(string projectId, string sprintId, [FromBody] AddSprintTaskRequest request)
    {
        // Implementation for adding a task to a specific sprint in the backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpGet("{projectId}/backlog/sprints/{sprintId}/tasks")]
    public async Task<IActionResult> GetTasksFromSprint(string projectId, string sprintId)
    {
        // Implementation for getting tasks from a specific sprint in the backlog of the specified project
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    // ... Additional methods as needed ...
}