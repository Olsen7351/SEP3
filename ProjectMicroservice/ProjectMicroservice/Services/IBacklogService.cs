using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface IBacklogService
    {
        Backlog GetBacklogByProjectId(int projectId);

        Backlog CreateBacklog(int projectId, CreateBacklogRequest backlog);
        bool ProjectHasBacklog(int projectId);
    }
}
