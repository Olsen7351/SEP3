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
            // Convert projectId to ObjectId
            ObjectId id = new();
            if (!ObjectId.TryParse(projectId, out var objectId))
            {
                id = objectId;
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
            ObjectId id = new();
            if (!ObjectId.TryParse(projectId, out var objectId))
            {
                id = objectId;
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
    }
}
