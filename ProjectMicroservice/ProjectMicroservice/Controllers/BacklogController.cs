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
        private readonly ITaskService _taskService;

        
        public BacklogController(IBacklogService backlogService, IProjectService projectService,
            ITaskService taskService)
        {
            _backlogService = backlogService;
            _projectService = projectService;
            _taskService = taskService;
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
        
      
        

        [HttpPost("BackLogTask")]
        public IActionResult AddTask(int projectId, [FromBody] CreateTaskRequest request)
        {
            ObjectId id;
            if (!ObjectId.TryParse(projectId.ToString(), out id))
            {
                return BadRequest("Id not found");
            }

            if (!_projectService.ProjectExists(id))
            {
                return NotFound("Project not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var taskToBeAdded = new Models.Task
            {
                ProjectId = id,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow, 
            };
            var createdTask = _taskService.CreateTask(taskToBeAdded);
            if (createdTask == null)
            {
                return StatusCode(500, "There was a problem with  the request");
                
            }

            return Ok(createdTask);
        }

        [HttpDelete("Task/{taskId}")]
        public IActionResult DeleteTask(string taskId)
        {
            ObjectId id;
            if (!ObjectId.TryParse(taskId, out id))
            {
                return BadRequest("Invalid Task id");
            }

            var existingTask = _taskService.GetTask(id);
            if (existingTask == null)
            {
                return NotFound("Task not found");
            }

            bool isDeleted = _taskService.DeleteTask(id);
            if (!isDeleted)
            {
                return StatusCode(500, "There was an issue with deleting the task");
            }

            return NoContent();
        }
        
    }
}
