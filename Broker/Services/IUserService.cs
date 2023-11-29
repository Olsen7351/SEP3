using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public interface IUserService
{
    public Task<IActionResult> CreateUser(CreateUserRequest user);
    
    public Task<IActionResult> LoginWithUserCredentials (User user);
}