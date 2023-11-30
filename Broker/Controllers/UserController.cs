using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
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
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
    {
        if (user == null)
        {
            return new BadRequestResult();
        }

        var serviceResult = await _IuserService.CreateUser(user);

        if (serviceResult is OkObjectResult okResult)
        {
            return Ok(okResult.Value);
        }
        else
        {
            return serviceResult;
        }
    }



    [HttpPost("Login")]
    public async Task<IActionResult> LoginWithUserCredentials(User user)
    {
        if (user == null)
        {
            return BadRequest("User data is required.");
        }
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _IuserService.LoginWithUserCredentials(user);

        if (result != null) // Assuming 'result' is the actual result you'd get from _IuserService
        {
            return
                Ok("Login successful"); // Make sure you're returning a string here, not result.Value or something else
        }

        return BadRequest("Invalid login attempt.");
    }
    
}

