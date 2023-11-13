using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Http.HttpResults;
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
        private readonly IProjectService _projectService;

        public TaskController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(string projectId, [FromBody] AddBacklogTaskRequest request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProject = _projectService.GetProject(projectId);
            string objecid = ObjectId.GenerateNewId().ToString() ?? string.Empty;

            // Create the new task
            var taskToBeAdded = new Task
            {
                Id = objecid,
                ProjectId = projectId,
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] string projectId, [FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProject = _projectService.GetProject(projectId);

            existingProject.Backlog.BacklogTasks.RemoveAll(task => task.Id == id);

            // Save the updated project with the task removed
            _projectService.UpdateProject(existingProject);

            return new OkResult();
        }
    }
}
