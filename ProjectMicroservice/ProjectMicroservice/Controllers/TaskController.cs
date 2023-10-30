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
        private readonly IBacklogService _backlogService;

        public TaskController(ITaskService taskService, IBacklogService backlogService)
        {
            _taskService = taskService;
            _backlogService = backlogService;
        }

        [HttpPost]
        public IActionResult CreateTask(string projectId, string backlogId, [FromBody] CreateTaskRequest request)
        {
            ObjectId projectObjectId, backlogObjectId;
            if (!ObjectId.TryParse(projectId, out projectObjectId) || !ObjectId.TryParse(backlogId, out backlogObjectId))
            {
                return BadRequest("Invalid project or backlog id");
            }

            if (!_backlogService.BacklogBelongsToProject(backlogObjectId, projectObjectId))
            {
                return NotFound("Specified backlog does not belong to the specified project.");
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
                Status = request.Status
            };

            var createdTask = _taskService.CreateTask(task);
            return CreatedAtAction(
                nameof(CreateTask),
                new
                {
                    projectId,
                    backlogId,
                    id = createdTask.Id,
                    status = createdTask.Status
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
        
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(string projectId, string backlogId, string id)
        {
            ObjectId projectObjectId, backlogObjectId, taskObjectId;
            if (!ObjectId.TryParse(projectId, out projectObjectId) ||
                !ObjectId.TryParse(backlogId, out backlogObjectId) ||
                !ObjectId.TryParse(id, out taskObjectId))
            {
                return BadRequest("Invalid project, backlog, or task id");
            }

            // Check if the backlog belongs to the project
            if (!_backlogService.BacklogBelongsToProject(backlogObjectId, projectObjectId))
            {
                return NotFound("Specified backlog does not belong to the specified project.");
            }

            var task = _taskService.GetTask(taskObjectId);
            if (task == null)
            {
                return NotFound("Task not found.");
            }

            // Ensure the task belongs to the specified backlog and project
            if (task.ProjectId != projectObjectId || task.BacklogId != backlogObjectId)
            {
                return NotFound("Task not found under the specified backlog and project.");
            }

            _taskService.DeleteTask(taskObjectId);
            return NoContent(); // 204 No Content is typically returned when a delete operation is successful.
        }
    }
}
