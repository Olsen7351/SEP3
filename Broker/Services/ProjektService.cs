using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

namespace Broker.Services
{
    public class ProjektService : IProjektService
    {
        private readonly HttpClient httpClient;

        public ProjektService(HttpClient httpClient)
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

        public async Task<ActionResult<Project>> GetProjekt(int id)
        {
            if (id < 0)
            {
                return new BadRequestResult();
            }
            else
            {
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
}