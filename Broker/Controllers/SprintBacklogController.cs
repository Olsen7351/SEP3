using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Broker.Controllers
{
    [Route("api/[controller]/{ProjectId}")]
    [ApiController]
    public class SprintBacklogController : ControllerBase
    {
        private readonly ISprintBacklogService _sprintBacklogService;

        public SprintBacklogController(ISprintBacklogService sprintBacklogService)
        {
            _sprintBacklogService = sprintBacklogService;
        }


        // GET: api/<SprintBacklogController>/<ProjectId>
        [HttpGet]
        public async Task<IActionResult> GetAllSprintBacklogs(string ProjectId)
        {
            //TODO Get all SprintBacklogs
            return new OkObjectResult(new List<SprintBacklog>());
        }

        // GET api/<SprintBacklogController>/<ProjectId>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificSprintBacklog(string ProjectId, string id)
        {
            //TODO Get a SprintBacklog by Id
            return new OkObjectResult(new SprintBacklog());
        }

        // POST api/<SprintBacklogController>/<ProjectId>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SprintBacklog value)
        {
            //TODO Create a SprintBacklog
            return new OkObjectResult(new SprintBacklog());
        }

        // PUT api/<SprintBacklogController>/<ProjectId>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            //TODO Update a SprintBacklog by Id
        }

        // DELETE api/<SprintBacklogController>/<ProjectId>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            //TODO Delete a SprintBacklog by Id
        }
    }
}
