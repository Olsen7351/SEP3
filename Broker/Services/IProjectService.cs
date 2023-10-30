using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.Models;

namespace Broker.Services
{
    public interface IProjectService
    {
        public Task<ActionResult<Project>> GetProjekt(string id);
        public Task<ActionResult> CreateProjekt(Project projekt);
    }
}