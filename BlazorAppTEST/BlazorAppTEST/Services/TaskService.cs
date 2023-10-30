namespace BlazorAppTEST.Services;

public class TaskService
{
    //HTTPClient
    private readonly HttpClient httpClient;
    
    public TaskService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    /*
    public async Task GetTaskByBacklogID(int BacklogID)
    {
        HttpResponseMessage response = await httpClient.PostAsync("/api/BacklogTask", BacklogID);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {responseContent}");
        }
    }
    */
    
    
   /* public async Task CreateTask(Task task)
    {
        HttpResponseMessage response = await httpClient.PostAsync("/api/CreateTask", task);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {responseContent}");
        }
    }
    */

    /*public async Task DeleteSelectedTask(Task task)
    {
        HttpResponseMessage response = await httpClient.PostAsync("/api/DeleteTask", task);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {responseContent}");
        }
    }
    */
}