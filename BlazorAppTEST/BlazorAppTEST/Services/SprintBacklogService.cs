﻿using System.Text;
using System.Text.Json;
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
            string createSprintBacklogToJson = JsonSerializer.Serialize(sprintBacklog);
            StringContent contentSprint =
                new StringContent(createSprintBacklogToJson, Encoding.UTF8, "application/json");
            HttpResponseMessage message = await _httpClient.PostAsync("api/SprintBacklog", contentSprint);
            if (message.IsSuccessStatusCode)
            {
                return new OkObjectResult(await message.Content.ReadAsStringAsync());
            }
            else
            {
                return new StatusCodeResult((int)message.StatusCode);
            }
        }

        public async Task<IActionResult> GetSprintBacklogsAsync(string ProjectId)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/SprintBacklog/{ProjectId}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var sprintBacklog = JsonSerializer.Deserialize<SprintBacklog>(content);
                return new OkObjectResult(sprintBacklog);
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        }

        public async Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/SprintBacklog/{ProjectId}/{Id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return new OkObjectResult(content);
            }
            else
            {
                return new StatusCodeResult((int)response.StatusCode);
            }
        
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
