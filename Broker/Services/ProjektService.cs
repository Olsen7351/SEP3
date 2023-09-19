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
        
        public ActionResult CreateProjekt(Projekt projekt)
        {
            if (projekt == null)
            {
                return new BadRequestResult();
            } else if (projekt.Id < 0)
            {
                return new BadRequestResult();
            } else {
                httpClient.PostAsJsonAsync("api/Projekt", projekt);
            }


            throw new NotImplementedException();
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
                Projekt response = httpClient.GetFromJsonAsync<Projekt>(requestUri).Result;
                if (response == null)
                {
                    return new NotFoundResult();
                }
                return response;
            }
        }
    }
}