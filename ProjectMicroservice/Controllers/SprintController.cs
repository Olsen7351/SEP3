
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
  
    [HttpPost]
    public IActionResult CreateSprint([FromBody] CreateSprintBackLogRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var createdSprint = _sprintService.CreateSprintBacklog(request);
        if (createdSprint == null)
        {
            return BadRequest("Sprint could not be created");
        }

        return CreatedAtAction(
            nameof(GetSpecificSprint), 
            new { projectId = createdSprint.ProjectId, sprintId = createdSprint.SprintBacklogId }, 
            createdSprint
        );
    }
    [HttpGet("{projectId}/backlog")]
    public IActionResult GetSprintBacklog(string projectId, string sprintBacklogId)
    {
        var sprintBacklog = _sprintService.GetSprintBacklogById(sprintBacklogId);
        if (sprintBacklog == null)
        {
            return BadRequest("Sprint could not be Found");
        }
        return Ok(sprintBacklog); 
        
    }

    [HttpGet("{projectId}/SprintBacklogs")]
    public IActionResult GetAllSprints(string projectId)
    {
        var sprints = _sprintService.GetAllSprintBacklogs(projectId);
        if (sprints == null)
        {
            return BadRequest($"No sprints found for project with Id {projectId}.");
        }
        return Ok(sprints);
    }

    [HttpGet("{projectId}/backlog/sprints/{sprintId}")]
    public IActionResult GetSpecificSprint(string sprintId)
    {
        var sprint = _sprintService.GetSprintBacklogById(sprintId);
        if (sprint == null)
        {
            return BadRequest($"No sprint was found sprintbacklog Id {sprintId}");
        }

        return Ok(sprint);

    }

    [HttpPut("{projectId}/backlog/sprints/{sprintId}")]
    public IActionResult UpdateSprint(string projectId, string sprintId, [FromBody] SprintBacklog sprint)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedSprint = _sprintService.UpdateSprintBacklog(sprintId, sprint);
        if (updatedSprint == null)
        {
            return BadRequest("The updated sprint is null");
        }

        return Ok(updatedSprint);
    }

    [HttpDelete("{projectId}/{sprintId}")]
    public IActionResult DeleteSprint(string projectId, string sprintBacklogId)
    {
        Console.WriteLine("Microservice controller Delete Sprint called");

        var success = _sprintService.DeleteSprintBacklog(projectId, sprintBacklogId);
        if (!success)
        {
            return BadRequest($"Sprint with ID {sprintBacklogId} not found.");
        }

        return NoContent();
    }

    [HttpPost("AddTask")]
    public IActionResult AddTaskToSprint( [FromBody] AddSprintTaskRequest request)
    {        
        Console.WriteLine("Addtask called in microservices");

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedSprint = _sprintService.AddTaskToSprintBacklog(request, request.SprintId);
        if (updatedSprint == null)
        {
            return BadRequest($"Task could not be added");
        }

        return Ok(updatedSprint);
    }

    [HttpGet("{projectId}/backlog/sprints/{sprintId}/tasks")]
    public IActionResult GetTasksFromSprint(string projectId, string sprintId)
    {
        var tasks = _sprintService.GetAllTasksForSprintBacklog(projectId, sprintId);
        if (tasks == null)
        {
            return BadRequest("The tasks could not be retrieved");
        }

        return Ok(tasks);
    }

}