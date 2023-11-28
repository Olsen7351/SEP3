using ProjectMicroservice.DataTransferObjects;

namespace BlazorAppTEST.Services;

public interface ITaskService
{
    Task<ClassLibrary_SEP3.Task?> CreateTask(AddBacklogTaskRequest task);
    Task DeleteTask(string id, string projectId);
}