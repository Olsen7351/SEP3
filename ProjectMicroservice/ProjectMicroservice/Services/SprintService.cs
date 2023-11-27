using ClassLibrary_SEP3;
<<<<<<< HEAD

namespace ProjectMicroservice.Services;
=======
using ClassLibrary_SEP3.DataTransferObjects;

namespace DefaultNamespace;
>>>>>>> Tests2

public class SprintService : ISprintService
{
<<<<<<< HEAD
    public SprintBacklog createSprintBacklog(SprintBackLogRequest sprintBacklog)
=======
    public List<SprintBacklog> GetAllSprintBacklogs(string projectId)
>>>>>>> Tests2
    {
        throw new NotImplementedException();
    }

<<<<<<< HEAD
    public SprintBacklog getSprintBacklog(SprintBacklog sprintBacklog)
=======
    public SprintBacklog GetSprintBacklogById(string sprintBacklogId)
    {
        throw new NotImplementedException();
    }

    public SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request)
    {
        throw new NotImplementedException();
    }

    public SprintBacklog UpdateSprintBacklog(string id, SprintBacklog sprintBacklog)
    {
        throw new NotImplementedException();
    }

    public bool DeleteSprintBacklog(string id)
>>>>>>> Tests2
    {
        throw new NotImplementedException();
    }
}