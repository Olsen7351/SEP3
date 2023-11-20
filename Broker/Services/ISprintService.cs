using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace Broker_Test;

public interface ISprintService
{
    public Task<List<SprintBackLogRequest>> GetSprintBacklog(String projectId, string backlogId);
    public Task<IActionResult> CreateSprintBacklog(SprintBackLogRequest sprintBack);
}