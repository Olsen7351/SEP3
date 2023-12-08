

using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using ProjectMicroservice.Services;

using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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

        [HttpGet("User/{username}/Projects")]
        public IActionResult GetProjectsByUser(string username)
        {
            var projects = _projectService.GetProjectsByUser(username);

            if (projects == null || !projects.Any())
            {
                return NotFound("No projects found for the specified user.");
            }

            return Ok(projects);
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

        //api/Project/{projectIdAsString}/Members
        [HttpGet("{projectIdAsString}/Members")]
        public IActionResult GetProjectMembers(string projectIdAsString)
        {
            var response = _projectService.GetProjectMembers(projectIdAsString);

            if (response == null)
            {
                return NotFound();
            }
            return new OkObjectResult(response);
        }


        [HttpGet("{id}")]
        public Project GetProject(string id)
        {
            Project project = _projectService.GetProject(id);


            return project;
        }

        [HttpPost("{projectId}/addUser")]
        public IActionResult AddUserToProject([FromBody] AddUserToProjectRequest request)
        {
            Console.WriteLine("Microservice Controller Adduser was called");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _projectService.AddUserToProject(request);

            if (!result)
            {
                return NotFound();
            }

            return Ok(new { Message = "User added to project successfully." });
        }
    }
}
