using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public interface IProjectService
{
    public Task<IActionResult> CreateProject(CreateProjectRequest project);
    Task<Project> GetProject(string id);
    
}