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

    
    public async Task CreateTask()
    {
        
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