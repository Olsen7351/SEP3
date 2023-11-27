using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _IuserService;


    public UserController(IUserService iuserService)
    {
        _IuserService = iuserService;
    }

    
    //Create User
    [HttpPost]
    public async Task<IActionResult> CreateUser(User user)
    {
        if (user == null)
        {
            return new BadRequestResult();
        }

        return Ok(await _IuserService.CreateUser(user));
    }
    
    
    
    [HttpPost("Login")]
    public async Task<IActionResult> LoginWithUserCredentials(User user)
    {
        if (user == null)
        {
            return BadRequest("User data is required.");
        }

        return await _IuserService.LoginWithUserCredentials(user);
    }
}