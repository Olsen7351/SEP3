﻿using System.Text;
using System.Text.Json;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using Xunit.Sdk;
using Task = System.Threading.Tasks.Task;


namespace BlazorAppTEST.Services;

public class ProjectService: IProjectService
{
   //HTTPClient
   private readonly HttpClient httpClient; 

    public ProjectService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    
    //Post
    public async Task<IActionResult> CreateProject(CreateProjectRequest project)
    {
        if (String.IsNullOrEmpty(project.Name))
        {
            throw new Exception("Project name cant be empty");
        }
        
        
        //Try and send it trough
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/BrokerProject", project);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }

        return new OkResult();
    }


    public async Task<Project> GetProject(string id)
    {
        var response = await httpClient.GetAsync($"api/BrokerProject/{id}");
        var projekt = await response.Content.ReadFromJsonAsync<Project>();
        if (projekt == null)
        {
            throw new Exception("Project is empty or do not exsist");
        }

        Console.WriteLine(response.Content);
        
        return projekt;
    }

    

    public async Task<Project> AddUserToProject(string username, string projectId)
    {
        var payload = new { Username = username, ProjectId = projectId };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/AddUserToProject", payload);

        if (response.IsSuccessStatusCode)
        {
            var project = await response.Content.ReadFromJsonAsync<Project>();
            return project;
        }
        
        else
        {
            throw new Exception($"Error: {response.StatusCode}");
        }
    }
}