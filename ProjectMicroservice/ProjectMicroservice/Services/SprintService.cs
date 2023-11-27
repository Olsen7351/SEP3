

using ClassLibrary_SEP3;
using DefaultNamespace;

namespace ProjectMicroservice.Services;
using ClassLibrary_SEP3.DataTransferObjects;

public class SprintService : ISprintService
{


    public List<SprintBacklog> GetAllSprintBacklogs(string projectId)
    {
        throw new NotImplementedException();
    }
    

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

    {
        throw new NotImplementedException();
    }
}