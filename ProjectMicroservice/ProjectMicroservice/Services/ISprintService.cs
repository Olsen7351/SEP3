using ClassLibrary_SEP3;

namespace ProjectMicroservice.Services;

public interface ISprintService
{
    SprintBacklog createSprintBacklog(SprintBackLogRequest sprintBacklog);
    SprintBacklog getSprintBacklog(SprintBacklog sprintBacklog);
}