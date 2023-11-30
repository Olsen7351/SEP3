using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;

namespace Broker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrokerProjectController : ControllerBase
    {
        private readonly IProjectService projektService;

        public BrokerProjectController(IProjectService projektService)
        {
            this.projektService = projektService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjekt(string id)
        {
            var response = await projektService.GetProjekt(id);
            return new OkObjectResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProjekt([FromBody] CreateProjectRequest projekt)
        {
            if (projekt == null)
            {
                return new BadRequestResult();
            }

            return Ok(await projektService.CreateProjekt(projekt));
        }

        [HttpPost("AddUserToProject")]
        public async Task<IActionResult> AddUserToProject([FromBody] AddUserToProjectRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new BadRequestResult();
                }
                
                await projektService.AddUserToProject( request);


                return Ok(await projektService.AddUserToProject(request));
            }
            catch (Exception ex)
            {
                // Handle specific exceptions if necessary
                return BadRequest($"Failed to add user to project: {ex.Message}");
            }
        }
    }

}