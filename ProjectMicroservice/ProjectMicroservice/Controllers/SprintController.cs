using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace;

[Route("api/[controller]")]
[ApiController]
public class SprintController : ControllerBase
{
    public ISprintService ProjectSprint;

    public SprintController(ISprintService sprintService)
    {
        ProjectSprint = sprintService;
    }
    [HttpPost]
        public async Task<IActionResult> CreateTask(string projectId, [FromBody] CreateSprintBacklogRequest request)
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