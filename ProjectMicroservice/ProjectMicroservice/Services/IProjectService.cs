using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface IProjectService
    {
        ProjectDatabase CreateProject(CreateProjectRequest project);
        ProjectDatabase GetProject(ObjectId id);
        bool ProjectExists(ObjectId projectId);
    }
}
