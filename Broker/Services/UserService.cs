using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace;

public class UserService : IUserService
{
    private readonly HttpClient _client;

    public UserService(HttpClient client)
    {
        _client = client;
    }

    public Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> GetUser(string username)
    {
        throw new NotImplementedException();
    }
}