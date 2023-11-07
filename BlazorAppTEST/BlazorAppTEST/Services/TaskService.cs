using System.Reflection.Metadata;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;
using Task = ClassLibrary_SEP3.Task;

namespace BlazorAppTEST.Services;

public class TaskService
{
    //HTTPClient
    private readonly HttpClient httpClient;
    
    public TaskService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
   public async Task<Task?> CreateTask(AddBacklogTaskRequest task)
   {
       string Url = $"api/Backlog/{task.ProjectId}/Backlog/Add";
       var response = await httpClient.PostAsJsonAsync(Url,task);
       
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
        var taskResponse = response.Content.ReadFromJsonAsync<Task>().Result;

        return taskResponse;
   }
    

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