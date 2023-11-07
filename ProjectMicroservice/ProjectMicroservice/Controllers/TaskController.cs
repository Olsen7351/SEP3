using ProjectMicroservice.Models;
using ProjectMicroservice.Services;
using ProjectMicroservice.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Task = ClassLibrary_SEP3.Task;

namespace ProjectMicroservice.Controllers
{
    [Route("api/Project/{projectId}/Backlog/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public TaskController(ITaskService taskService, IProjectService projectService)
        {
            _taskService = taskService;
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(string projectId, [FromBody] AddBacklogTaskRequest request)
        {
            ObjectId projectObjectId;
            if (!ObjectId.TryParse(projectId, out projectObjectId))
            {
                return BadRequest("Invalid projectId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProject = _projectService.GetProject(projectObjectId);

            // Create the new task
            var taskToBeAdded = new Task
            {
                ProjectId = projectObjectId,
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                EstimateTime = request.EstimateTime,
                Responsible = request.Responsible,
            };
            // Add the new task to the project's backlog
            if (existingProject.Backlog.BacklogTasks == null)
            {
                existingProject.Backlog = new Backlog
                {
                    BacklogTasks = new List<Task>()
                };
            }
            existingProject.Backlog.BacklogTasks.Add(taskToBeAdded);
            // Save the updated project with the added task back to the database
            var createdTask = _projectService.UpdateProject(existingProject);

            return new OkObjectResult(taskToBeAdded);
        }
    }
}
