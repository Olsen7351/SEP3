using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.Models;

namespace Broker.Services
{
    public interface IProjectService
    {
        public Task<IActionResult> GetProjekt(string id);
        public Task<IActionResult> CreateProjekt(Project projekt);
    }
}