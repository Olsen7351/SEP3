using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using ClassLibrary_SEP3.RabbitMQ;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
namespace Broker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BrokerProjectController : ControllerBase
    {
        private readonly IProjectService projektService;

        public BrokerProjectController(IProjectService projektService)
        {
            this.projektService = projektService;
        }

        [HttpGet("{id}")]
        public async Task<Project> GetProjekt(string id)
        {
            var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);

            var response = await projektService.GetProjekt(id);

            if (response == null)
            {
                Logger.LogMessage(username +": Error getting project: "+id);
                throw new Exception("Project is empty or do not exsist");
            }
            Logger.LogMessage(username +": Got project: "+id);
            return response;
        }

        //api/BrokerProject/{projectIdAsString}/Members
        [HttpGet("{projectIdAsString}/Members")]
        public async Task<List<string>> GetProjectMembers(string projectIdAsString)
        {
            var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            var response = await projektService.GetProjectMembers(projectIdAsString);

            if (response == null)
            {
                Logger.LogMessage(username +": Error getting project members: "+projectIdAsString);
                throw new Exception("Project is empty or do not exsist");
            }
            Logger.LogMessage(username +": Got project members: "+projectIdAsString);
            return response;
        }

        
        [HttpPost("CreateProject")]
        public async Task<IActionResult> CreateProjekt([FromBody] CreateProjectRequest projekt)
        {
            var token = HttpContext.Request.Headers["Authorization"];
            Console.WriteLine($"Token used to access: {token}");

            // Get the 'sub' claim from the JWT token
            string? usernameClaim = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


            if (projekt == null)
            {
                Logger.LogMessage(usernameClaim+": Error creating project: "+projekt);
                return new BadRequestResult();
            }

            
            // Check if the usernameClaim matches the ByUsername parameter
            if (usernameClaim == null || usernameClaim != projekt.ByUsername)
            {
                Logger.LogMessage(usernameClaim+": Error creating project, Unauthorized: "+projekt);
                return new UnauthorizedResult();
            }

            // Proceed with project creation
            var result = await projektService.CreateProjekt(projekt);
            Logger.LogMessage(usernameClaim+": Created project: "+projekt);
            return Ok(result);
        }

        [HttpPost("AddUserToProject")]
        public async Task<IActionResult> AddUserToProject([FromBody] AddUserToProjectRequest request)
        {
            var username  = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);

            try
            {
                if (request == null)
                {
                    Logger.LogMessage(username +": Error adding user to project: "+request);
                    return new BadRequestResult();
                }
                
                var response = await projektService.AddUserToProject( request);
                Logger.LogMessage(username +": Added user to project: "+request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle specific exceptions if necessary
                return BadRequest($"Failed to add user to project: {ex.Message}");
            }
        }

        [HttpGet("User/{username}/Projects")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjectsByUser(string username)
        {
            var username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            try
            {
                var projects = await projektService.GetProjectsByUser(username);

                if (projects == null || !projects.Any())
                {
                    Logger.LogMessage(username +": No projects found for the specified user.");
                    return NotFound("No projects found for the specified user.");
                }

                Logger.LogMessage(username +": Got projects for user: "+username);
                return Ok(projects);
            }
            catch (Exception ex)
            {
                Logger.LogMessage(username +": Error getting projects for user: "+ex.Message);
                // Handle specific exceptions if necessary
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }

    }

}