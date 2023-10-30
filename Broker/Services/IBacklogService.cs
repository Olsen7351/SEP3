using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

namespace Broker.Services;

public interface IBacklogService
{
    public Task<IActionResult> GetBacklog(int projectId);
    public Task<IActionResult> CreateBacklog(Backlog backlog);
    public Task<IActionResult> AddTaskToBackLog(int projectId, BackLogTask task);
}