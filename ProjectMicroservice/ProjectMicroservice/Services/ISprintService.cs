using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
namespace DefaultNamespace;

public interface ISprintService
{
    List<SprintBacklog> GetAllSprintBacklogs(string projectId);
    SprintBacklog GetSprintBacklogById(string sprintBacklogId);
    SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request);
    SprintBacklog UpdateSprintBacklog(string id, SprintBacklog sprintBacklog);
    bool DeleteSprintBacklog(string id);
}