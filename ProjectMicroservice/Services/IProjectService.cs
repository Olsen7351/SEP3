using ClassLibrary_SEP3;

using Microsoft.AspNetCore.Mvc;

using ClassLibrary_SEP3.DataTransferObjects;

using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using ProjectDatabase = ClassLibrary_SEP3.Project;

namespace ProjectMicroservice.Services
{
    public interface IProjectService
    {
        Project CreateProject(CreateProjectRequest project);
        Project GetProject(string id);
        bool ProjectExists(string projectId);
        Project UpdateProject(Project project);
        bool AddUserToProject(AddUserToProjectRequest request);
        List<string> GetProjectMembers(string projectIdAsString);
    }
}
