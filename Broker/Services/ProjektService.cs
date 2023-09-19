using Broker.Shared_Classes;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services
{
    public class ProjektService : IProjektService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ProjektService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public ActionResult CreateProjekt(Projekt projekt)
        {
            throw new NotImplementedException();
        }

        public ActionResult<Projekt> GetProjekt(int id)
        {
            throw new NotImplementedException();
        }
    }
}