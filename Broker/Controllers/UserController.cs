using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[ApiController]
[Route("api/Broker/[controller]" )]
public class UserController : ControllerBase
{
    private readonly IUserService _IuserService;


    public UserController(IUserService iuserService)
    {
        _IuserService = iuserService;
    }


    //Create User
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest user)
    {
        if (user == null)
        {
            return new BadRequestResult();
        }

        var serviceResult = await _IuserService.CreateUser(user);

        return serviceResult;
    }



    [HttpPost("Login")]
    public async Task<IActionResult> LoginWithUserCredentials(User user)
    {
        var result = await _IuserService.LoginWithUserCredentials(user);
        //Get the token from the result and return it
        if (result is OkObjectResult okResult)
        {

            return Ok(okResult.Value);
        }
        else
        {
            return BadRequest();
        }
    }
    
}

