
using ProjectMicroservice.Models;
using ProjectMicroservice.Services;

using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;

namespace ProjectMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody] CreateProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Returns a 400 Bad Request with validation errors.
            }
            
            if (request.StartDate > request.EndDate)
            {
                ModelState.AddModelError("StartDate", "Start date must be before end date.");
                return BadRequest(ModelState);
            }

            var createdProject = _projectService.CreateProject(request);
            return CreatedAtAction(nameof(CreateProject), new { id = createdProject.Id }, createdProject);
        }

        [HttpGet("{id}")]
        public IActionResult GetProject(string id)
        {
            var project = _projectService.GetProject(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
    }
}
