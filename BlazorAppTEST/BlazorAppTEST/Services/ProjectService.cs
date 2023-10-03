using System.Collections;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

namespace MainWeb.Services;

public class ProjectService
{
    //HTTPClient
    private readonly HttpClient httpClient;


    public async Task CreateProjekt(Project projekt)
    {
        //Http client
        using HttpClient client = new();
        //Takes project and serialize to json
        string createProjectToJson = JsonSerializer.Serialize(projekt);
        //Magic
        StringContent contentProject = new(createProjectToJson, Encoding.UTF8, "application/json");
       
        //Try and send it trough
        HttpResponseMessage response = await client.PostAsync("https://localhost:5172/", contentProject);
        string responseContent = await response.Content.ReadAsStringAsync();
       
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {contentProject}");
        }
    }


    
    
    
    
    //Get
    public async Task<ICollection<Project>> GetProjekt()
    {
        HttpResponseMessage response = await httpClient.GetAsync(("http://localhost:5172/"));
        string contentProject = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}, {contentProject}");
        }

        ICollection<Project> projects = JsonSerializer.Deserialize<ICollection<Project>>(contentProject, new JsonSerializerOptions());
        return projects;
    }
}