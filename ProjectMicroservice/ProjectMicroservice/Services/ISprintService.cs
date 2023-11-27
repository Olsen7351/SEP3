using ClassLibrary_SEP3;
<<<<<<< HEAD

namespace ProjectMicroservice.Services;

public interface ISprintService
{
    SprintBacklog createSprintBacklog(SprintBackLogRequest sprintBacklog);
    SprintBacklog getSprintBacklog(SprintBacklog sprintBacklog);
=======
using ClassLibrary_SEP3.DataTransferObjects;
namespace DefaultNamespace;

public interface ISprintService
{
    List<SprintBacklog> GetAllSprintBacklogs(string projectId);
    SprintBacklog GetSprintBacklogById(string sprintBacklogId);
    SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request);
    SprintBacklog UpdateSprintBacklog(string id, SprintBacklog sprintBacklog);
    bool DeleteSprintBacklog(string id);
>>>>>>> Tests2
}