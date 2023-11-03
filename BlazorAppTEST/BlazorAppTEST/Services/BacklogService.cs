﻿using System.Text;
using System.Text.Json;
using ProjectMicroservice.Models;
namespace BlazorAppTEST.Services;
using ClassLibrary_SEP3;

public class BacklogService
{

    private List<Task> tasks = new List<Task>();
    //HTTPClient
    private readonly HttpClient httpClient;

    public BacklogService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    //Post
    public async Task<Backlog> CreateBacklog(Backlog backlog)
    {
        string createBacklogToJson = JsonSerializer.Serialize(backlog);
        StringContent contentBacklog = new StringContent(createBacklogToJson, Encoding.UTF8, "application/json");
   
        HttpResponseMessage response = await httpClient.PostAsync("/api/Backlog", contentBacklog);
        string responseContent = await response.Content.ReadAsStringAsync();
    
        if (response.IsSuccessStatusCode)
        {
            return JsonSerializer.Deserialize<Backlog>(responseContent);
        }
        else
        {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }
    }



    public Task GetTaskById(string taskId)
    {
        return tasks.FirstOrDefault(task => task.TaskID == taskId);
    }

    public void UpdateTask(Task task)
    {
        throw new NotImplementedException();
    }
}