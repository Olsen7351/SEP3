using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Broker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

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
            if (id < 0)
            {
                return new BadRequestResult();
            }
            return Ok(projektService.GetProjekt(id));
        }

        [HttpPost]
        public IActionResult CreateProjekt([FromBody] Project projekt)
        {
            if (projekt == null)
            {
                return new BadRequestResult();
            }
            return Ok(projektService.CreateProjekt(projekt));
        }

    }

}