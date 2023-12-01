using System.Threading.Tasks;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Broker.Services
{
    public interface IProjectService
    {
        public Task<Project> GetProjekt(string id);
        public Task<IActionResult> CreateProjekt(CreateProjectRequest projekt);
    }
}