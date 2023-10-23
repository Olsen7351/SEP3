using Microsoft.AspNetCore.Http.HttpResults;
using ProjectMicroservice.Models;
using ProjectMicroservice.Services;
using ProjectMicroservice.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ProjectMicroservice.Controllers
{
    [Route("api/Project/{projectId}/[controller]")]
    [ApiController]
    public class BacklogController : ControllerBase
    {
        private readonly IBacklogService _backlogService;
        private readonly IProjectService _projectService;  // Injecting IProjectService

        public BacklogController(IBacklogService backlogService, IProjectService projectService)
        {
            _backlogService = backlogService;
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult GetBacklogByProjectId(string projectId)
        {
            ObjectId id;
if (!ObjectId.TryParse(projectId, out id))
{
    return BadRequest("Invalid project id");
}
            var backlog = _backlogService.GetBacklogByProjectId(id);
            if (backlog == null)
            {
                return NotFound();
            }
            return Ok(backlog);
        }

        [HttpPost]
        public IActionResult CreateBacklog(string projectId, [FromBody] CreateBacklogRequest request)
        {
            // Convert projectId to ObjectId
            ObjectId id;
            if (!ObjectId.TryParse(projectId, out id))
            {
                return BadRequest("Invalid project id");
            }
            if (!_projectService.ProjectExists(id))
            {
                return NotFound("Project not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_backlogService.ProjectHasBacklog(id))
            {
                return Conflict("This project already has a backlog.");
            }

            var createdBacklog = _backlogService.CreateBacklog(id, request);
            return CreatedAtAction(nameof(CreateBacklog), new { projectId, id = createdBacklog.Id }, createdBacklog);
        }
        
        [HttpDelete("BackLogTask/taskId")]
        public IActionResult DeleteTask(int projectId, int taskId)
        {
            if (!_projectService.ProjectExists(projectId))
            {
                return NotFound("Project not found");
            }

            if (!_backlogService.ProjectHasBacklog(projectId))
            {
                return NotFound("No backlog for this project");
            }

            var delete = _backlogService.DeleteTask(projectId, taskId);
            if (!delete)
            {
                return NotFound("BackLogTask not found");
            }

            return NoContent();
        }

        [HttpPost("BackLogTask")]
        public IActionResult AddTask(int projectId, [FromBody] AddBacklogTaskRequest request)
        {
            if (!_projectService.ProjectExists(projectId))
            {
                return NotFound("Project not found");
            }

            if (!ModelState.IsValid)
            {
                return NotFound("No backlog for the project");
            }

            var addTask = _backlogService.AddTask(projectId, request.TaskId, request.Title);
            if (!addTask)
            {
                return Conflict("There is already a task like this");
            }

            return CreatedAtAction(nameof(addTask), new { projectId, taskId = request.TaskId }, request);
        }
        
    }
}
