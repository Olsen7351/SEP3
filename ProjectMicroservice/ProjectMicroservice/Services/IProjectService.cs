using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services;

public interface IProjectService
{
    Project CreateProject(Project project);
}