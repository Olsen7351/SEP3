using ProjectMicroservice.Models;
using ProjectMicroservice.Services;
using ProjectMicroservice.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ProjectMicroservice.Controllers
{
    [Route("api/Project/{projectId}/Backlog/{backlogId}/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public IActionResult CreateTask(string projectId, string backlogId, [FromBody] CreateTaskRequest request)
        {
            ObjectId projectObjectId, backlogObjectId;
            if (!ObjectId.TryParse(projectId, out projectObjectId) || !ObjectId.TryParse(backlogId, out backlogObjectId))
            {
                return BadRequest("Invalid project or backlog id");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = new Models.TaskDatabase
            {
                ProjectId = projectObjectId,
                BacklogId = backlogObjectId,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = System.DateTime.UtcNow
        };

            var createdTask = _taskService.CreateTask(task);
            return CreatedAtAction(
                nameof(CreateTask),
                new
                {
                    projectId,
                    backlogId,
                    id = createdTask.Id,
                    status = createdTask.Status,
                    CreatedAt = System.DateTime.UtcNow
                },
                createdTask
            );
        }

        [HttpGet("{id}")]
        public IActionResult GetTask(string projectId, string backlogId, string id)
        {
            ObjectId taskId;
            if (!ObjectId.TryParse(id, out taskId))
            {
                return BadRequest("Invalid task id");
            }

            var task = _taskService.GetTask(taskId);
            if (task == null)
            {
                return NotFound();
            }

            // Additional validation to ensure the task belongs to the specified backlog and project
            if (task.ProjectId != ObjectId.Parse(projectId) || task.BacklogId != ObjectId.Parse(backlogId))
            {
                return NotFound("Task not found under the specified backlog and project.");
            }

            return Ok(task);
        }
    }
}
