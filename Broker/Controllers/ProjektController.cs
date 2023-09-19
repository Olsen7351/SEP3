using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Broker.Services;
using Broker.Shared_Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjektController : ControllerBase
    {
        private readonly IProjektService projektService;

        public ProjektController(IProjektService projektService)
        {
            this.projektService = projektService;
        }

        [HttpGet("{id}")]
        public IActionResult GetProjekt([FromBody] int id)
        {
            return Ok(projektService.GetProjekt(id));
        }

        [HttpPost]
        public IActionResult CreateProjekt([FromBody] Projekt projekt)
        {
            return Ok(projektService.CreateProjekt(projekt));
        }

    }

}