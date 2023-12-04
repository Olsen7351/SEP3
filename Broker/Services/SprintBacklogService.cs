using Broker.Controllers;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Xunit.Sdk;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Services;

public class SprintBacklogService : ISprintBacklogService

{
    private readonly HttpClient httpClient;

    public SprintBacklogService(HttpClient client)
    {
        this.httpClient = client;
    }

    public async Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog)
    {
        string requestUri = $"api/Project/{sprintBacklog.ProjectId}/SprintBacklog";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, sprintBacklog);
        if (response.IsSuccessStatusCode)
        {
            return new CreatedAtActionResult(nameof(SprintBacklogController.GetSpecificSprintBacklog), "SprintBacklog",
                new { id = sprintBacklog.SprintBacklogId }, sprintBacklog);
        }

        return new BadRequestResult();
    }
    public async Task<IActionResult> GetSprintBacklogsAsync(string projectId)
    {
        string requestUri = $"api/Project/{projectId}/SprintBacklog";
        HttpResponseMessage responseMessage = await httpClient.GetAsync(requestUri);
        if (responseMessage.IsSuccessStatusCode)
        {
            var sprintBacklogs = await responseMessage.Content.ReadFromJsonAsync<List<SprintBacklog>>();
            if (sprintBacklogs == null || !sprintBacklogs.Any()) 
            {
                return new NotFoundResult(); 
            }
            return new OkObjectResult(sprintBacklogs);
        }
        return new BadRequestResult();
    }
    public async Task<IActionResult> GetSprintBacklogByIdAsync(string projectId, string Id)
    {
        string requestUri = $"api/Project/{projectId}/SprintBacklog/{Id}";
        HttpResponseMessage responseMessage = await httpClient.GetAsync(requestUri);
        if (responseMessage.IsSuccessStatusCode)
        {
            var sprintBacklog = await responseMessage.Content.ReadFromJsonAsync<SprintBacklog>();
            return new OkObjectResult(sprintBacklog);
        }
        return new BadRequestResult();
    }
    public async Task<IActionResult> UpdateSprintBacklogAsync(string projectId, string id, SprintBacklog sprintBacklog)
    {
        string requestUri = $"api/Project/{projectId}/SprintBacklog/{id}";
        HttpResponseMessage response = await httpClient.PutAsJsonAsync(requestUri, sprintBacklog);
        if (response.IsSuccessStatusCode)
        {
            return new OkObjectResult(sprintBacklog);
        }
        return new BadRequestResult();
    }
    public async Task<IActionResult> DeleteSprintBacklogAsync(string projectId, string id)
    {
        string requestUri = $"api/Project/{projectId}/SprintBacklog/{id}";
        HttpResponseMessage response = await httpClient.DeleteAsync(requestUri);
        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }
        return new BadRequestResult();
    }

    public async Task<IActionResult> AddTaskToSprintBacklogAsync(AddSprintTaskRequest task)
    {
        if (string.IsNullOrWhiteSpace(task.ProjectId) || string.IsNullOrWhiteSpace(task.SprintId))
        {
            return new BadRequestResult();
        }

        string requestUri = $"api/Project/{task.ProjectId}/SprintBacklog/{task.SprintId}/tasks";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, task);
    
        if (response.IsSuccessStatusCode)
        {
            return new OkObjectResult(await response.Content.ReadFromJsonAsync<SprintBacklog>());
        }

        return new BadRequestResult();
    }

    public Task<IActionResult> GetTasksFromSprintBacklogAsync(string projectId, string Id)
    {
        //TODO 
        throw new NotImplementedException();
    }
    
}