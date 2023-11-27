using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Broker.Controllers
{
    [Route("api/")]
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
            return await _sprintBacklogService.GetSprintBacklogsAsync(ProjectId);
        }

        // GET api/<SprintBacklogController>/<ProjectId>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificSprintBacklog(string ProjectId, string id)
        {
            return await _sprintBacklogService.GetSprintBacklogByIdAsync(ProjectId, id);
        }

        // POST api/<SprintBacklogController>/<ProjectId>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SprintBacklog sprintBacklog)
        {
            return await _sprintBacklogService.CreateSprintBacklogAsync(sprintBacklog);
        }

        // PUT api/<SprintBacklogController>/<ProjectId>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string Projectid, string id, [FromBody] SprintBacklog sprintBacklog)
        {
            return await _sprintBacklogService.UpdateSprintBacklogAsync(Projectid, id, sprintBacklog);
        }

        // DELETE api/<SprintBacklogController>/<ProjectId>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string ProjectId, string id)
        {
            return await _sprintBacklogService.DeleteSprintBacklogAsync(ProjectId, id);
        }
    }
}
