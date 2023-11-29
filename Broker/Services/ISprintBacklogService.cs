﻿using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Services
{
    public interface ISprintBacklogService
    {
        public Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog);
        public Task<IActionResult> GetSprintBacklogsAsync(string ProjectId);
        public Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id);
        Task<IActionResult> UpdateSprintBacklogAsync(string projectId, string id, SprintBacklog sprintBacklog);
        Task<IActionResult> DeleteSprintBacklogAsync(string projectId, string id);
        public Task<IActionResult> AddTaskToSprintBacklogAsync(string projectId, string id, Task task);
        public Task<IActionResult> GetTasksFromSprintBacklogAsync(string projectId, string Id);
    }
}
