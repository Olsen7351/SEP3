using System.Collections.Concurrent;
using DefaultNamespace;
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

        public bool DeleteTask(int projectId, int taskId)
        {
            if (_backlogs.TryGetValue(projectId, out var backlog))
            {
                var removeTask = backlog.Tasks.FirstOrDefault(t => t.Id == taskId);

                if (removeTask != null)
                {
                    backlog.Tasks.Remove(removeTask);
                    return true;
                }
            }

            return false;
        }

        public bool AddTask(int projectId, int taskId, string title)
        {
            if (_backlogs.TryGetValue(projectId, out var backlog))
            {
                if (backlog.Tasks.Any(t => t.Id == taskId))
                {
                    return false;
                }

                var newTask = new BackLogTask
                {
                    Id = taskId,
                    Title = title,
                    Description = "",
                    isCompleted = false,
                    Responsible = ""
                };
                backlog.Tasks.Add(newTask);
                return true;
            }

            return false;
        }
        
    }
}
