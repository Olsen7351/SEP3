using ClassLibrary_SEP3;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
=======
using ClassLibrary_SEP3.DataTransferObjects;
>>>>>>> Test3
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using ProjectMicroservice.Tests;
using ProjectDatabase = ClassLibrary_SEP3.Project;

namespace ProjectMicroservice.Services
{
    public interface IProjectService
    {
        Project CreateProject(CreateProjectRequest project);
        Project GetProject(string id);
        bool ProjectExists(string projectId);
        Project UpdateProject(Project project);
        bool AddUserToProject(string projectId, string userName);
    }
}
