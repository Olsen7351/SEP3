﻿using System.Reflection.Metadata;
<<<<<<< HEAD
using BlazorAppTEST.Services.Interface;
=======
>>>>>>> 88775a473578bea3f63a1222ee554c9e09398857
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services;

public class TaskService : ITaskService
{
    //HTTPClient
    private readonly HttpClient httpClient;
    
    public TaskService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
   public async Task<ClassLibrary_SEP3.Task?> CreateTask(AddBacklogTaskRequest task)
   {
       string Url = $"api/{task.ProjectId}/Backlog/AddTask";
       var response = await httpClient.PostAsJsonAsync(Url,task);
       
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
        var taskResponse = response.Content.ReadFromJsonAsync<ClassLibrary_SEP3.Task>().Result;

        return taskResponse;
   }
    

   public async Task DeleteTask(string id, string projectId)
    {
        string Url = $"api/{projectId}/Backlog/Task/{id}";
        var response = await httpClient.DeleteAsync(Url);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
    }
    
}