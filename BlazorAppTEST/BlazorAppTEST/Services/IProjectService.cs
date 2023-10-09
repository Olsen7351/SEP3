using ProjectMicroservice.Models;

namespace BlazorAppTEST.Services
{
    public interface IProjectService
    {
        public Task CreateProject(Project projekt);
        public Task<ICollection<Project>?> GetAllProjects();

    }
}
