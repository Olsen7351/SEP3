using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services.Interface;

public interface IBacklogService
{
    Task<Backlog> CreateBacklog(Backlog backlog);
    
    
}