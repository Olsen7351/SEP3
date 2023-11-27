using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public interface IUserService
{
    public Task<IActionResult> CreateUser(User user);
    
    public Task<IActionResult> LoginWithUserCredentials (User user);
}