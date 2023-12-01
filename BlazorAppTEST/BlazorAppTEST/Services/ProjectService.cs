using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
<<<<<<< HEAD
using BlazorAppTEST.Services.Auth;
=======
using BlazorAppTEST.Services.Interface;
>>>>>>> Test3
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        //Add JWT token to the header
        this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserService.Jwt);
    }

    
    //Post
    public async Task<IActionResult> CreateProject(CreateProjectRequest project)
    {
        if (String.IsNullOrEmpty(project.Name))
        {
            throw new Exception("Project name cant be empty");
        }
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserService.Jwt);
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
        if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(projectId))
        {
            throw new Exception("Either username or projectID couldn't be retrieved");
        }
        
        // Service to check maybe if username exists inside the database
        
        var payload = new { Username = username, ProjectId = projectId };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Broker/AddUserToProject", payload);

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