using ProjectMicroservice.Models;
using System.Text;
using System.Text.Json;

namespace BlazorAppTEST.Services
{

    public class ProjectService : IProjectService
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
            //Magic
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
    }

}
