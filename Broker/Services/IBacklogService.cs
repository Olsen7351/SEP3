using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Services;

public interface IBacklogService
{
    public Task<IActionResult> GetBacklog(int projectId);
    public Task<IActionResult> CreateBacklog(Backlog backlog);
    public Task<IActionResult> AddTaskToBackLog(int projectId, Task task);
    public Task<IActionResult> DeleteTaskFromBacklog(int projectId,int backlogId, DeleteBacklogTaskRequest task);
}