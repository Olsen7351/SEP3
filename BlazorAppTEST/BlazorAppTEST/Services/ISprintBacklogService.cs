using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services
{
    public interface ISprintBacklogService
    {
        public Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog);
        public Task<IActionResult> GetSprintBacklogsAsync(string ProjectId);
        public Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id);
    }
}
