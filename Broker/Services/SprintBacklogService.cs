using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public class SprintBacklogService : ISprintBacklogService
{
    public Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> GetSprintBacklogsAsync(string ProjectId)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id)
    {
        throw new NotImplementedException();
    }
}