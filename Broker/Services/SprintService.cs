using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace Broker_Test;

public class SprintService : ISprintService
{
    public SprintService(HttpClient httpClient)
    {
        throw new NotImplementedException();
    }

    public  Task<SprintBackLogRequest> GetSprintBacklog(string id)
    {
        throw new NotImplementedException();
    }

    public Task<List<SprintBackLogRequest>> GetSprintBacklog(string projectId, string backlogId)
    {
        throw new NotImplementedException();
    }

    public Task<IActionResult> CreateSprintBacklog(SprintBackLogRequest sprintBack)
    {
        throw new NotImplementedException();
    }
}