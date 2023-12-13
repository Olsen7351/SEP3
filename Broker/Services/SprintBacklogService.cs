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

    public async Task<IActionResult> CreateSprintBacklogAsync(CreateSprintBackLogRequest sprintBacklog)
    {
        if (sprintBacklog == null)
        {
            return new BadRequestResult();
        }
        string requestUri = $"api/Sprint";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"api/Sprint", sprintBacklog);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        return new OkObjectResult(responseBody);
    }
    public async Task<IActionResult> GetSprintBacklogsAsync(string projectId)
    {
        
        string requestUri = $"api/Sprint/{projectId}/SprintBacklogs";
        HttpResponseMessage responseMessage = await httpClient.GetAsync(requestUri);
        if (responseMessage.IsSuccessStatusCode)
        {
            var sprintBacklogs = await responseMessage.Content.ReadFromJsonAsync<List<SprintBacklog>>();
            if (sprintBacklogs == null || !sprintBacklogs.Any()) 
            {
                //return new NotFoundResult(); 
                sprintBacklogs = new List<SprintBacklog>();
            }
            return new OkObjectResult(sprintBacklogs);
        }
        else
        {
            Console.WriteLine(responseMessage.Content.ToString());
        }

        throw new Exception($"Error:{responseMessage.StatusCode}");
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
    public async Task<IActionResult> DeleteSprintBacklogAsync(string projectId, string sprintId)
    {
        Console.WriteLine("Broker service Delete Sprint called");

        string requestUri = $"api/Sprint/{projectId}/{sprintId}";
        HttpResponseMessage response = await httpClient.DeleteAsync($"api/Sprint/{projectId}/{sprintId}");
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

        HttpResponseMessage response = await httpClient.PostAsJsonAsync($"api/Sprint/AddTask", task);
    
        if (response.IsSuccessStatusCode)
        {
            return new OkObjectResult(await response.Content.ReadFromJsonAsync<SprintBacklog>());
        }

        return new BadRequestResult();
    }

    public async Task<IActionResult> GetTasksFromSprintBacklogAsync(string projectId, string sprintBacklogId)
    {
        string requestUri = $"api/Project/{projectId}/SprintBacklog/{sprintBacklogId}/tasks";
        HttpResponseMessage responseMessage = await httpClient.GetAsync(requestUri);
        if (responseMessage.IsSuccessStatusCode)
        {
            var tasks = await responseMessage.Content.ReadFromJsonAsync<List<Task>>();
            if (tasks == null || !tasks.Any()) 
            {
                return new NotFoundResult(); 
            }
            return new OkObjectResult(tasks);
        }
        return new BadRequestResult();
    }
    
}