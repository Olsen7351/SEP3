using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services.Interface
{
    public interface ISprintBacklogService
    {
        public Task<IActionResult> CreateSprintBacklogAsync(CreateSprintBackLogRequest sprintBacklog);
        public Task<IActionResult> GetSprintBacklogsAsync(string ProjectId);
        public Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id);

        public Task<IActionResult> AddTaskToSprintBacklogAsync(
            AddSprintTaskRequest task);

        public Task<IActionResult> GetTasksFromSprintBacklogAsync(string projectId, string Id);
    }
}
