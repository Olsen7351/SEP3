﻿using System.Text;
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
        public async Task<IActionResult> CreateSprintBacklogAsync(CreateSprintBackLogRequest sprintBacklog)
        {
            try
            {
                HttpResponseMessage message = await _httpClient.PostAsJsonAsync($"api/SprintBacklog/", sprintBacklog);
                if (!message.IsSuccessStatusCode)
                {
                    var errorContent = await message.Content.ReadAsStringAsync();
                    return new BadRequestObjectResult($"Server responded with error: {message.StatusCode} - {errorContent}");
                }

                string content = await message.Content.ReadAsStringAsync();
                if (string.IsNullOrWhiteSpace(content))
                {
                    return new BadRequestObjectResult("Received empty response from server.");
                }

                var sprintBacklogResponse = JsonSerializer.Deserialize<SprintBacklog>(content);
                return new OkObjectResult(sprintBacklogResponse);
            }
            catch (JsonException ex)
            {
                return new BadRequestObjectResult($"JSON deserialization error: {ex.Message}");
            }
        }
        public async Task<IActionResult> GetSprintBacklogsAsync(string ProjectId)
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<SprintBacklog>>("api/SprintBacklog?ProjectId=" + ProjectId);
            if (response == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(response);
        }
        public async Task<IActionResult> GetSprintBacklogByIdAsync( string sprintBacklogId)
        {
            var response = await _httpClient.GetFromJsonAsync<SprintBacklog>($"api/SprintBacklog/{sprintBacklogId}");
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

            HttpResponseMessage message = await _httpClient.PostAsJsonAsync($"api/SprintBacklog/{task.SprintId}/AddTask", task);
            if (message.IsSuccessStatusCode)
            {
                return new OkObjectResult(await message.Content.ReadFromJsonAsync<AddSprintTaskRequest>());
            }
            return new NotFoundResult();
        }

        public async Task<IActionResult> GetTasksFromSprintBacklogAsync(string sprintId)
        {
            if (string.IsNullOrWhiteSpace(sprintId))
            {
                throw new HttpRequestException("Sprint backlog not found.");
            }

            var response = await _httpClient.GetFromJsonAsync<IEnumerable<Task>>($"api/SprintBacklog/{sprintId}/Tasks");
            if (response == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(response);
        }
    }
}
