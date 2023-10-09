using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

namespace Broker.Services;

public interface IBacklogService
{
    public Task<IActionResult> GetBacklog(int projectId);
    public Task<IActionResult> CreateBacklog(Backlog backlog);
}