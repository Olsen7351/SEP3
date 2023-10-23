using System.Collections;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;
using Task = System.Threading.Tasks.Task;

namespace MainWeb.Services;

public class ProjectService
{
    //HTTPClient
    private readonly HttpClient httpClient;

    public ProjectService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    
    //Post
    public async Task CreateProject(Project projekt)
    {
        //Takes project and serialize to json
        string createProjectToJson = JsonSerializer.Serialize(projekt);
        StringContent contentProject = new(createProjectToJson, Encoding.UTF8, "application/json");
       
        //Try and send it trough
        HttpResponseMessage response = await httpClient.PostAsync("/api/Projekt", contentProject);
        string responseContent = await response.Content.ReadAsStringAsync();
       
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {responseContent}");
        }
    }

    
    
    
    //Get All
    public async Task<ICollection<Project>?> GetAllProjects()
    {
        HttpResponseMessage response = await httpClient.GetAsync(("/"));
        string contentProject = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {contentProject}");
        }

        ICollection<Project>? projects = JsonSerializer.Deserialize<ICollection<Project>>(contentProject, new JsonSerializerOptions());
        return projects;
    }
    
    
    
    
    //Get Backlog for Project
    public async Task<string?> GetBacklogIDForProject(string projectId)
    {
        HttpResponseMessage response = await httpClient.GetAsync($"/api/Project/{projectId}/BacklogID");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {content}");
        }
        return content;
    }

}