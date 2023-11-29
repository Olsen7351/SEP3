using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace Broker.Services
{
    public interface ISprintBacklogService
    {
        public Task<IActionResult> CreateSprintBacklogAsync(CreateSprintBackLogRequest sprintBacklog);
        public Task<IActionResult> GetSprintBacklogsAsync(string ProjectId);
        public Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id);
        Task<IActionResult> UpdateSprintBacklogAsync(string projectId, string id, SprintBacklog sprintBacklog);
        Task<IActionResult> DeleteSprintBacklogAsync(string projectId, string id);
    }
}
