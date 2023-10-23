using Microsoft.AspNetCore.Http.HttpResults;
using ProjectMicroservice.Models;
using ProjectMicroservice.Services;
using ProjectMicroservice.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult GetBacklogByProjectId(int projectId)
        {
            var backlog = _backlogService.GetBacklogByProjectId(projectId);
            if (backlog == null)
            {
                return NotFound();
            }
            return Ok(backlog);
        }

        [HttpPost]
        public IActionResult CreateBacklog(int projectId, [FromBody] CreateBacklogRequest request)
        {
            if (!_projectService.ProjectExists(projectId))
            {
                return NotFound("Project not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_backlogService.ProjectHasBacklog(projectId))
            {
                return Conflict("This project already has a backlog.");
            }

            var backlog = new Backlog
            {
                ProjectId = projectId,
                Description = request.Description,
                // other fields
            };

            var createdBacklog = _backlogService.CreateBacklog(projectId, backlog);
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
