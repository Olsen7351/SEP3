using System.Threading.Tasks;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.Models;

namespace Broker.Services
{
    public interface IProjectService
    {
        public Task<Project> GetProjekt(string id);
        public Task<IActionResult> CreateProjekt(Project projekt);
    }
}