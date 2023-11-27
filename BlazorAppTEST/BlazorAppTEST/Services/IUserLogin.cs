using ClassLibrary_SEP3;

namespace BlazorAppTEST.Services;

public interface IUserLogin
{
    Task<User> Login(User user);
}