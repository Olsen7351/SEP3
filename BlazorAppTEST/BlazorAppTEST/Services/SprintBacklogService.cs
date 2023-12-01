using System.Text;
using System.Text.Json;
using BlazorAppTEST.Services.Interface;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

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
<<<<<<< HEAD
                return new NotFoundResult();
=======
                string content = await response.Content.ReadAsStringAsync();
                var sprintBacklog = JsonSerializer.Deserialize<SprintBacklog>(content);
                return new OkObjectResult(sprintBacklog);
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
>>>>>>> Test3
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

        public Task<IActionResult> AddTaskToSprintBacklogAsync(string projectId, string sprintId,
            AddSprintTaskRequest task)
        {
            //TODO
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetTasksFromSprintBacklogAsync(string projectId, string Id)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
