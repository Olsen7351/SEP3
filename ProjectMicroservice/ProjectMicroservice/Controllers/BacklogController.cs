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
        private readonly IProjectService _projectService;  // Injecting IProjectService
        private readonly ITaskService _taskService;

        
        public BacklogController( IProjectService projectService,
            ITaskService taskService)
        {
            _projectService = projectService;
            _taskService = taskService;
        }

        [HttpPost("BackLogTask")]
        public IActionResult AddTask(int projectId, [FromBody] AddBacklogTaskRequest request)
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
           
            var taskToBeAdded = new Models.TaskDatabase
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
