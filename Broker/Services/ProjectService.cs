using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace Broker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient httpClient;

        public ProjectService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IActionResult> CreateProjekt(CreateProjectRequest projekt)
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

            return await response.Content.ReadFromJsonAsync<Project>();
        }
    }
}