using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Broker.Services
{
    public class SprintBacklogService : ISprintBacklogService
    {
        private readonly HttpClient _httpClient;

        public SprintBacklogService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog)
        {
            //TODO Create a SprintBacklog
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetSprintBacklogsAsync(string ProjectId)
        {
            //TODO Get all SprintBacklogs
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id)
        {
            //TODO Get a SprintBacklog by Id
            throw new NotImplementedException();
        }
    }
}
