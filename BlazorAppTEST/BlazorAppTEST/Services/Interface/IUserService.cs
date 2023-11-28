using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public interface IUserService
{
    public Task createUser(User user);
    
}