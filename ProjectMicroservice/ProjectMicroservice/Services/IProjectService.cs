using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface IProjectService
    {
        Project CreateProject(CreateProjectRequest project);
        Project GetProject(ObjectId id);
        bool ProjectExists(ObjectId projectId);
    }
}
