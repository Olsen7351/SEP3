using System.Threading.Tasks;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Broker.Services
{
    public interface IProjectService
    {
        public Task<Project> GetProjekt(String id);
        public Task<IActionResult> CreateProjekt(Project projekt);
    }
}