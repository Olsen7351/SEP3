using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public interface IRequirementService
{
    Task AddRequirement(Requirement requirement);
    Task<List<Requirement>> GetRequirementsForBacklog(int backlogId);
}