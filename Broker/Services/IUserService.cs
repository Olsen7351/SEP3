using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace;

public interface IUserService
{
    public Task<IActionResult> CreateUser(CreateUserRequest request);
    public Task<ActionResult> GetUser(string username);
}