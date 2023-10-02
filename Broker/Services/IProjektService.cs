using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

namespace Broker.Services
{
    public interface IProjektService
    {
        public Task<ActionResult<Project>> GetProjekt(int id);
        public Task<ActionResult> CreateProjekt(Project projekt);
    }
}