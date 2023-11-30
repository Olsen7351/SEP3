﻿using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services
{
    public interface ISprintBacklogService
    {
        public Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog);
        public Task<IActionResult> GetSprintBacklogsAsync(string ProjectId);
        public Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id);

        public Task<IActionResult> AddTaskToSprintBacklog(string projectId, string sprintId,
            AddSprintTaskRequest task);

        public Task<IActionResult> GetTasksFromSprintBacklogAsync(string projectId, string Id);
    }
}
