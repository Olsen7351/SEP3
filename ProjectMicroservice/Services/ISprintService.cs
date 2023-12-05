
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using ProjectDatabase = ClassLibrary_SEP3.Project;
using Task = ClassLibrary_SEP3.Task;

namespace ProjectMicroservice.Services;
public interface ISprintService
{
    SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request);
    SprintBacklog GetSprintBacklogById(string sprintBacklogId);
    List<SprintBacklog> GetAllSprintBacklogs(string projectId);
    SprintBacklog UpdateSprintBacklog(string id, SprintBacklog updatedSprintBacklog);
    bool DeleteSprintBacklog(string projectId, string sprintBacklogId);
    SprintBacklog AddTaskToSprintBacklog(AddSprintTaskRequest request, string sprintBacklogId);
    List<Task> GetAllTasksForSprintBacklog(string projectId, string sprintBacklogId);
}