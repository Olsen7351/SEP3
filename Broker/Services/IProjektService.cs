using System.Threading.Tasks;
using Broker.Shared_Classes;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services
{
    public interface IProjektService
    {
        public Task<ActionResult<Projekt>> GetProjekt(int id);
        public Task<ActionResult> CreateProjekt(Projekt projekt);
    }
}