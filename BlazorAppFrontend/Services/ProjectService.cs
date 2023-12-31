﻿using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using BlazorAppTEST.Services.Auth;

using BlazorAppTEST.Services.Interface;

using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
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
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserService.Jwt);
    }

    
    //Post
    public async Task<IActionResult> CreateProject(CreateProjectRequest project)
    {
        if (String.IsNullOrEmpty(project.Name))
        {
            throw new Exception("Project name cant be empty");
        }
        Console.WriteLine($"Token used to access: {UserService.Jwt}");
        //Try and send it trough
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/BrokerProject/CreateProject", project);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }

        return new OkResult();
    }


    public async Task<Project> GetProject(string id)
    {
        var response = await httpClient.GetAsync($"api/BrokerProject/{id}");
        var project = await response.Content.ReadFromJsonAsync<Project>();
        
        if (project == null)
        {
            throw new Exception("Project is empty or do not exists");
        }

        Console.WriteLine(response.Content);
        
        return project;
    }

    

    public async Task<Project> AddUserToProject(string username, string projectId)
    {
        Console.WriteLine("Frontend Adduser was called");
        if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(projectId))
        {
            throw new Exception("Either username or projectID couldn't be retrieved");
        }
        
        // Service to check maybe if username exists inside the database TODO
        
        
        var payload = new { Username = username, ProjectId = projectId };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/BrokerProject/AddUserToProject", payload);

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

    public async Task<IEnumerable<Project>> GetProjectsByUser(string username)
    {
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("Username cannot be null or empty.");
        }

        var response = await httpClient.GetAsync($"api/BrokerProject/User/{username}/Projects");

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Error while fetching projects for user {username}. Status code: {response.StatusCode}");
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<Project>>();
    }

    public async Task<List<string>> GetProjectMembers(string projectIdAsString)
    {
        if (projectIdAsString.IsNullOrEmpty())
        {
            throw new Exception("ProjectId is empty");
        }

        HttpResponseMessage response = await httpClient.GetAsync($"api/BrokerProject/{projectIdAsString}/Members");
        if (response.IsSuccessStatusCode)
        {
            var members = await response.Content.ReadFromJsonAsync<List<string>>();
            return members;
        }
        else
        {
            throw new NullReferenceException($"Error: {response.StatusCode}");
        }
    }
}