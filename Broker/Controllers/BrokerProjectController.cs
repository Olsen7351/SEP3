using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;

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
    }

}