
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using ProjectMicroservice.Tests;
using ProjectDatabase = ClassLibrary_SEP3.Project;

namespace ProjectMicroservice.Services;
public interface ISprintService
{
    List<SprintBacklog> GetAllSprintBacklogs(string projectId);
    SprintBacklog GetSprintBacklogById(string sprintBacklogId);
    SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request);
    SprintBacklog UpdateSprintBacklog(string id, SprintBacklog sprintBacklog);
    bool DeleteSprintBacklog(string id);
}