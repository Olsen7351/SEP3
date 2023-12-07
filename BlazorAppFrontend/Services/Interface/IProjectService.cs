using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services.Interface;


public interface IProjectService
{
    public Task<IActionResult> CreateProject(CreateProjectRequest project);
    Task<Project> GetProject(string id);

    Task<Project> AddUserToProject(string username, string projectId);

    Task<List<string>> GetProjectMembers(string projectIdAsString);
}