using System.Threading.Tasks;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;

namespace Broker.Services
{
    public interface IProjectService
    {
        public Task<IActionResult> GetProjekt(string id);
        public Task<IActionResult> CreateProjekt(CreateProjectRequest projekt);
    }
}