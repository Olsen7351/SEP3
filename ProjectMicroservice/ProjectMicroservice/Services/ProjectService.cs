using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services;

public class ProjectService : IProjectService
{

    public Project CreateProject(Project project)
    {
        // TODO: Save to database, get Id and set it
        return project;
    }
}