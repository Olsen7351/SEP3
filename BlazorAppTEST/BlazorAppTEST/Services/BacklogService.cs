﻿using System.Text;
using System.Text.Json;
using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public class BacklogService
{
    //HTTPClient
    private readonly HttpClient httpClient;

    public BacklogService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    
    //Post
    public async Task CreateBacklog(Backlog backlog)
    {
        //Takes project and serialize to json
        string createBacklogToJson = JsonSerializer.Serialize(backlog);
        //Magic
        StringContent contentBacklog = new(createBacklogToJson, Encoding.UTF8, "application/json");
       
        //Try and send it trough
        HttpResponseMessage response = await httpClient.PostAsync("/api/Backlog", contentBacklog);
        string responseContent = await response.Content.ReadAsStringAsync();
       
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {responseContent}");
        }
    }
    
    //Get All
    public async Task<ICollection<Backlog>?> GetAllBacklog()
    {
        HttpResponseMessage response = await httpClient.GetAsync(("/"));
        string contentBacklog = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {contentBacklog}");
        }

        ICollection<Backlog>? backlogs = JsonSerializer.Deserialize<ICollection<Backlog>>(contentBacklog, new JsonSerializerOptions());
        return backlogs;
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