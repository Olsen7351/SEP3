using System.Collections.Concurrent;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public class ProjectService : IProjectService
    {
        private static readonly ConcurrentDictionary<int, Project> _projects = new();

        public Project CreateProject(Project project)
        {
            // Placeholder for a database call.'
            _projects.TryAdd(project.Id, project);
            return project;
        }

        public bool ProjectExists(int projectId)
        {
            return _projects.ContainsKey(projectId);
        }
    }
}
