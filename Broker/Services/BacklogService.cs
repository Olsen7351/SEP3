using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }

        return new BadRequestResult();
    }
}