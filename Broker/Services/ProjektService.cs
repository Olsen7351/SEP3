using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Broker.Shared_Classes;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services
{
    public class ProjektService : IProjektService
    {
        private readonly HttpClient httpClient;

        public ProjektService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ActionResult> CreateProjekt(Projekt projekt)
        {
            if (projekt == null)
            {
                return new BadRequestResult();
            }

            HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Projekt", projekt);

            if (response.IsSuccessStatusCode)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestResult();
            }
        }

        public async Task<ActionResult<Projekt>> GetProjekt(int id)
        {
            if (id < 0)
            {
                return new BadRequestResult();
            }
            else
            {
                string requestUri = $"api/Projekt/{id}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUri);

                if (response.IsSuccessStatusCode)
                {
                    var projekt = await response.Content.ReadFromJsonAsync<Projekt>();
                    
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
                    throw new HttpRequestException($"Failed getting the Projec. Status code: {response.StatusCode}.");
                }
            }
        }
    }
}