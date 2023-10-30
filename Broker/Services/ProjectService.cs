using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.Models;

namespace Broker.Services
{
    public class ProjectService : IProjectService
    {
        private readonly HttpClient httpClient;

        public ProjectService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ActionResult> CreateProjekt(Project projekt)
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

        public async Task<ActionResult<Project>> GetProjekt(string id)
        {
            if (id == null)
            {
                return new BadRequestResult();
            }
            
            string requestUri = $"api/Project/{id}";
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var projekt = await response.Content.ReadFromJsonAsync<Project>();
                    
                    if (projekt != null)
                    {
                        return projekt;
                    }
                    else
                    {
                        return new NotFoundResult();
                    }
                }
                else
                {
                    throw new HttpRequestException($"Failed getting the Project. Status code: {response.StatusCode}.");
                }
        }
    }
}