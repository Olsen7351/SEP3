using Broker.Services;
using ClassLibrary_SEP3;

using Microsoft.AspNetCore.Authorization;

using ClassLibrary_SEP3.DataTransferObjects;

using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;


namespace Broker.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class SprintBacklogController : ControllerBase
    {
        private readonly ISprintBacklogService _sprintBacklogService;

        public SprintBacklogController(ISprintBacklogService sprintBacklogService)
        {
            _sprintBacklogService = sprintBacklogService;
        }

        // POST api/<SprintBacklogController>/<ProjectId>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSprintBackLogRequest sprintBacklog)
        {
            Console.WriteLine("Broker create sprint called");
            return await _sprintBacklogService.CreateSprintBacklogAsync(sprintBacklog);
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

       

        // PUT api/<SprintBacklogController>/<ProjectId>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string Projectid, string id, [FromBody] SprintBacklog sprintBacklog)
        {
            return await _sprintBacklogService.UpdateSprintBacklogAsync(Projectid, id, sprintBacklog);
        }

        // DELETE api/<SprintBacklogController>/<ProjectId>/5
        [HttpDelete("{ProjectId}/{sprintId}")]
        public async Task<IActionResult> Delete(string ProjectId, string sprintId)
        {
            Console.WriteLine("Broker controller Delete Sprint called");

            return await _sprintBacklogService.DeleteSprintBacklogAsync(ProjectId, sprintId);
        }
        [HttpPost("{sprintId}/AddTask")]
        public async Task<IActionResult> AddTaskToSprintBacklog(AddSprintTaskRequest task)
        {
            Console.WriteLine("Addtask called in broker cont");
            return await _sprintBacklogService.AddTaskToSprintBacklogAsync(task);
        }

        // GET api/<SprintBacklogController>/<ProjectId>/<SprintId>/Tasks
        [HttpGet("{sprintId}/Tasks")]
        public async Task<IActionResult> GetAllTasksForSprintBacklog(string projectId, string sprintId)
        {
            return await _sprintBacklogService.GetTasksFromSprintBacklogAsync(projectId, sprintId);
        }
    }
}
