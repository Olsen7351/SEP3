using System.Text;
using System.Text.Json;
using ClassLibrary_SEP3;
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
    public async Task CreateProject(CreateProjectRequest projekt)
    {
       //Try and send it trough
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/BrokerProject", projekt);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
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
}