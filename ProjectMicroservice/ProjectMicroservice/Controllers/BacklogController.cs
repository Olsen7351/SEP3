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

            var createdBacklog = _backlogService.CreateBacklog(projectId, request);
            return CreatedAtAction(nameof(CreateBacklog), new { projectId, id = createdBacklog.Id }, createdBacklog);
        }
    }
}
