using ClassLibrary_SEP3;
using System.Security.Claims;
using ClassLibrary_SEP3.DataTransferObjects;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public interface IUserLogin
{
    Task Login(User user);
    public Task LogoutAsync();
    public Task<ClaimsPrincipal> GetAuthAsync();

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
    public Task createUser(CreateUserRequest user);
}