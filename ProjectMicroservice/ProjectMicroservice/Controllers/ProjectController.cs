

using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectMicroservice.Services;

using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;
using MongoDB.Bson;
using Newtonsoft.Json;
using Xunit.Sdk;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
            Project project = _projectService.GetProject(id);


            return new OkObjectResult(project);
        }

        [HttpPost("{projectId)/addUser")]
        public IActionResult AddUserToProject(string projectId, [FromBody] AddUserToProjectRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _projectService.AddUserToProject(projectId, request.UserName);
        
            if (result == null)
            {
                return NotFound(); 
            }

            return Ok(result);
        }
        
    }
}
