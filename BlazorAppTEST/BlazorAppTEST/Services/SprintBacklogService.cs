using System.Text;
using System.Text.Json;
using ClassLibrary_SEP3;
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
    }
}
