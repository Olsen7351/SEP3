using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjektControllerController : ControllerBase
    {
        private readonly ILogger<Controller> _logger;
        public ProjektControllerController(ILogger<Controller> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            
            Return Ok();
        }
    }

}