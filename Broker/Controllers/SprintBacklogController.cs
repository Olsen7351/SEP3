using Broker.Services;
using ClassLibrary_SEP3;

using Microsoft.AspNetCore.Authorization;

using ClassLibrary_SEP3.DataTransferObjects;
using ClassLibrary_SEP3.RabbitMQ;
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
            var userName = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            try
            {
                Logger.LogMessage(userName+": SprintBacklog created: "+sprintBacklog.projectId+ " : "+sprintBacklog.Title);
                return await _sprintBacklogService.CreateSprintBacklogAsync(sprintBacklog);
            }
            catch
            {
                Logger.LogMessage(userName+": Error creating SprintBacklog: "+sprintBacklog.projectId+ " : "+sprintBacklog.Title);
                return BadRequest();
            }
            
        }
        // GET: api/<SprintBacklogController>/<ProjectId>
        [HttpGet]
        public async Task<IActionResult> GetAllSprintBacklogs(string ProjectId)
        {
            var userName = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            try
            {
                Logger.LogMessage(userName+": SprintBacklogs requested for project: "+ProjectId);
                return await _sprintBacklogService.GetSprintBacklogsAsync(ProjectId);
            }
            catch
            {
                Logger.LogMessage(userName+": Error getting SprintBacklogs for project: "+ProjectId);
                return BadRequest();
            }
            
        }

        // GET api/<SprintBacklogController>/<ProjectId>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecificSprintBacklog(string ProjectId, string id)
        {
            var userName = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);

            try
            {
                Logger.LogMessage(userName+": SprintBacklog requested: "+id);
                return await _sprintBacklogService.GetSprintBacklogByIdAsync(ProjectId, id);
            }
            catch
            {
                Logger.LogMessage(userName+": Error getting SprintBacklog: "+id);
                return BadRequest();
            }
            
        }

       

        // PUT api/<SprintBacklogController>/<ProjectId>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string Projectid, string id, [FromBody] SprintBacklog sprintBacklog)
        {
            var userName = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            try
            {
                Logger.LogMessage(userName+": SprintBacklog updated: "+id);
                return await _sprintBacklogService.UpdateSprintBacklogAsync(Projectid, id, sprintBacklog);
            }
            catch
            {
                Logger.LogMessage(userName+": Error updating SprintBacklog: "+id);
                return BadRequest();
            }
            
        }

        // DELETE api/<SprintBacklogController>/<ProjectId>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string ProjectId, string id)
        {
            var userName = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            try
            {
                Logger.LogMessage(userName + ": SprintBacklog deleted: " + id);
                return await _sprintBacklogService.DeleteSprintBacklogAsync(ProjectId, id);
            }
            catch
            {
                Logger.LogMessage(userName+": Error deleting SprintBacklog: "+id);
                return BadRequest();
            }
        }
        [HttpPost("{sprintId}/AddTask")]
        public async Task<IActionResult> AddTaskToSprintBacklog(AddSprintTaskRequest task)
        {
            var userName = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            try
            {
                Logger.LogMessage(userName + ": Task added to SprintBacklog: " + task.SprintId);
                return await _sprintBacklogService.AddTaskToSprintBacklogAsync(task);
            }
            catch
            {
                Logger.LogMessage(userName+": Error adding Task to SprintBacklog: "+task.SprintId);
                return BadRequest();
            }
        }

        // GET api/<SprintBacklogController>/<ProjectId>/<SprintId>/Tasks
        [HttpGet("{sprintId}/Tasks")]
        public async Task<IActionResult> GetAllTasksForSprintBacklog(string projectId, string sprintId)
        {
            var userName = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);
            try
            {
                Logger.LogMessage(userName + ": Tasks requested for SprintBacklog: " + sprintId);
                return await _sprintBacklogService.GetTasksFromSprintBacklogAsync(projectId, sprintId);
            }
            catch
            {
                Logger.LogMessage(userName+": Error getting Tasks for SprintBacklog: "+sprintId);
                return BadRequest();
            }
        }
    }
}
