using System.Text.Json;

namespace BlazorAppTEST.Services;

public class TaskService
{
    //HTTPClient
    private readonly HttpClient httpClient;
    
    public TaskService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task GetTaskByBacklogID(int BacklogID)
    {
        HttpResponseMessage response = await httpClient.PostAsync("/api/BacklogTask", BacklogID);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {responseContent}");
        }
    }
    
    
    
    public async Task CreateTask(Task task)
    {
        HttpResponseMessage response = await httpClient.PostAsync("/api/CreateTask", task);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {responseContent}");
        }
    }
    
    
    public async Task<Task?> GetTask(int taskId)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"/api/Task/{taskId}");
        string contentTask = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {contentTask}");
        }

        Task? task = JsonSerializer.Deserialize<Task>(contentTask, new JsonSerializerOptions());
        return task;
    }

}