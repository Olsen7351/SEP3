using System.Collections.Concurrent;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public class BacklogService : IBacklogService
    {
        // Using a ConcurrentDictionary to simulate a database table.
        private static readonly ConcurrentDictionary<int, Backlog> _backlogs = new();

        public Backlog GetBacklogByProjectId(int projectId)
        {
            _backlogs.TryGetValue(projectId, out var backlog);
            return backlog;
        }

        public Backlog CreateBacklog(int projectId, Backlog backlog)
        {
            var newBacklog = new Backlog
            {
                ProjectId = projectId,
                Description = backlog.Description
            };

            _backlogs[projectId] = newBacklog;
            return newBacklog;
        }

        public bool ProjectHasBacklog(int projectId)
        {
            return _backlogs.ContainsKey(projectId);
        }
    }
}
