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

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        if (changePasswordRequest == null)
        {
            return BadRequest("Invalid request");
        }

        var jwt = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        if (string.IsNullOrEmpty(jwt))
        {
            return Unauthorized("JWT token is missing");
        }

        var serviceResult = await _IuserService.ChangeUserPassword(jwt, changePasswordRequest);

        if (serviceResult is OkResult)
        {
            return Ok("Password changed successfully");
        }
        return serviceResult;
    }
}