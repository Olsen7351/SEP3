using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace Broker_Test;

public class SprintBacklogService : ISprintBacklogService

{
    public SprintBacklogService(HttpClient httpClient)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> CreateSprintBacklogAsync(SprintBacklog sprintBacklog)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> GetSprintBacklogsAsync(string ProjectId)
    {
        throw new NotImplementedException();
    }

    public async Task<IActionResult> GetSprintBacklogByIdAsync(string ProjectId, string Id)
    {
        throw new NotImplementedException();
    }
}