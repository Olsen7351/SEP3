using ClassLibrary_SEP3;
using ProjectMicroservice.DataTransferObjects;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public interface IProjectService
{
    Task CreateProject(CreateProjectRequest project);
    Task<Project> GetProject(string id);
    
}