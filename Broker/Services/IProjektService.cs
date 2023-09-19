using Broker.Shared_Classes;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services
{
    public interface IProjektService
    {
        public ActionResult<Projekt> GetProjekt(int id);
        public ActionResult CreateProjekt(Projekt projekt);
    }
}