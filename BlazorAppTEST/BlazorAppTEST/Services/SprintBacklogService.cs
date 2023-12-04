using System.Text;
using System.Text.Json;
using BlazorAppTEST.Services.Interface;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;

namespace BlazorAppTEST.Services
{
    public class SprintBacklogService : ISprintBacklogService
    
    
    {
        private readonly HttpClient _httpClient;
        public SprintBacklogService(HttpClient httpclient)
        {
            this._httpClient = httpclient;
        }
        public async Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog)
        {
            HttpResponseMessage message = await _httpClient.PostAsJsonAsync("api/SprintBacklog", sprintBacklog);
            if (message.IsSuccessStatusCode)
            {
                return new OkObjectResult(await message.Content.ReadFromJsonAsync<SprintBacklog>());
            }
            return new NotFoundResult();
        }
        public async Task<IActionResult> GetSprintBacklogsAsync(string ProjectId)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<SprintBacklog>>($"api/SprintBacklog/{ProjectId}");
            if (response == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(response);
        }
        public async Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id)
        {
            var response = await _httpClient.GetFromJsonAsync<SprintBacklog>($"api/SprintBacklog/{ProjectId}/{Id}");
            if (response == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(response);
        }

        public async Task<IActionResult> AddTaskToSprintBacklogAsync(
            AddSprintTaskRequest task)
        {
            if (string.IsNullOrWhiteSpace(task.SprintId))
            {
                throw new HttpRequestException("Sprint backlog ID cannot be null.");
            }

            HttpResponseMessage message = await _httpClient.PostAsJsonAsync($"api/SprintBacklog/{task.ProjectId}/{task.SprintId}/tasks", task);
            if (message.IsSuccessStatusCode)
            {
                return new OkObjectResult(await message.Content.ReadFromJsonAsync<AddSprintTaskRequest>());
            }
            return new NotFoundResult();
        }

        public async Task<IActionResult> GetTasksFromSprintBacklogAsync(string projectId, string sprintId)
        {
            if (string.IsNullOrWhiteSpace(sprintId))
            {
                throw new HttpRequestException("Sprint backlog not found.");
            }

            var response = await _httpClient.GetFromJsonAsync<IEnumerable<Task>>($"api/SprintBacklog/{projectId}/{sprintId}/tasks");
            if (response == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(response);
        }
    }
}
