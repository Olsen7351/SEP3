

using ClassLibrary_SEP3.DataTransferObjects;

namespace BlazorAppTEST.Services.Interface;


public interface ITaskService
{
    Task<ClassLibrary_SEP3.Task?> CreateTask(AddBacklogTaskRequest task);
    Task DeleteTask(string id, string projectId);
}