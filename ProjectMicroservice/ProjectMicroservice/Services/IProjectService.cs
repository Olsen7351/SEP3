using ClassLibrary_SEP3;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Tests;
using ProjectDatabase = ClassLibrary_SEP3.Project;

namespace ProjectMicroservice.Services
{
    public interface IProjectService
    {
        Project CreateProject(CreateProjectRequest project);
        Project GetProject(ObjectId id);
        bool ProjectExists(ObjectId projectId);
        Project UpdateProject(Project project);
    }
}
