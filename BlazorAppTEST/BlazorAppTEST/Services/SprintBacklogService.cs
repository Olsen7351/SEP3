using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services
{
    public class SprintBacklogService : ISprintBacklogService
    {
        public async Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog)
        {
            //TODO Create a SprintBacklog
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetSprintBacklogsAsync(string ProjectId)
        {
            //TODO Get all SprintBacklogs
            throw new NotImplementedException();
        }

        public async Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id)
        {
            //TODO Get a SprintBacklog by Id
            throw new NotImplementedException();
        }
    }
}
