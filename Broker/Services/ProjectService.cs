using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Xunit.Sdk;


namespace Broker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient httpClient;

        public ProjectService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> CreateProjekt(Project projekt)
        {
            if (projekt == null)
            {
                return new BadRequestResult();
            }

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Project", projekt);

            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        public async Task<Project> GetProjekt(string id)
        {
            string requestUri = $"api/Project/{id}";
            var response = await httpClient.GetAsync(requestUri);
            var projekt = await response.Content.ReadFromJsonAsync<Project>();
            if (projekt == null)
            {
                throw new Exception("Could not get project");
            }

            return projekt;
        }
    }
}