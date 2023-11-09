
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using ClassLibrary_SEP3.DataTransferObjects;
using ProjectMicroservice.DataTransferObjects;
using Xunit.Sdk;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Services;

public class BacklogService : IBacklogService
{
    private readonly HttpClient httpClient;

    public BacklogService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<Task> AddTaskToBackLog(string projectId, AddBacklogTaskRequest? task)
    {
        string requestUri = $"api/Project/{projectId}/Backlog/Task";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, task);
        var taskResponse = response.Content.ReadFromJsonAsync<Task>().Result;
        if (response.IsSuccessStatusCode)
        {
            return taskResponse;
        }

        throw new Exception("Failed to add task");
    }
    public async Task<IActionResult> DeleteTaskFromBacklog(string id, string ProjectId)
    {
        string requestUri = $"api/Project/{ProjectId}/Backlog/Task/{id}";
        HttpResponseMessage response = await httpClient.DeleteAsync(requestUri);
        var taskResponse = response.Content.ReadFromJsonAsync<Task>();
        if (response.IsSuccessStatusCode)
        {
            return new OkObjectResult(taskResponse);
        }
        else
        {
            return new BadRequestResult();
        }
    }
}