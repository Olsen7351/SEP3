using ClassLibrary_SEP3;

namespace DefaultNamespace;

public interface IProjectSprint
{
    SprintBacklog createSprintBacklog(SprintBackLogRequest sprintBacklog);
    SprintBacklog getSprintBacklog(SprintBacklog sprintBacklog);
}