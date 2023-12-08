using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using ClassLibrary_SEP3.RabbitMQ;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[ApiController]

[Route("api/Broker/[controller]")]
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
            Logger.LogMessage(user.Username + ": Error creating user: " + user.Username);
            return new BadRequestResult();
        }

        var serviceResult = await _IuserService.CreateUser(user);
        Logger.LogMessage("User created: " + user.Username);

        return serviceResult;
    }



    [HttpPost("Login")]
    public async Task<IActionResult> LoginWithUserCredentials(User user)
    {
        // Guard clause: Check for null user
        if (user == null)
        {
            return BadRequest("Invalid user credentials");
        }

        // Guard clause: Check for null or empty username
        if (string.IsNullOrEmpty(user.Username))
        {
            return BadRequest("Username is required");
        }

        // Guard clause: Check for null or empty password
        if (string.IsNullOrEmpty(user.Password))
        {
            return BadRequest("Password is required");
        }
        // Guard clause: Check for too long username
        if (user.Username.Length > 16)
        {
            return BadRequest("Username is too long, only 16 characters are allowed");
        }

        var result = await _IuserService.LoginWithUserCredentials(user);

        if (result is OkObjectResult okResult)
        {
            Logger.LogMessage(user.Username + ": User logged in: " + user.Username);
            // Successful login, return the token
            return Ok(okResult.Value);
        }
        else
        {
            Logger.LogMessage(user.Username + ": Error logging in: " + user.Username);
            // Login failed, return a generic BadRequest response
            return BadRequest("Invalid user credentials");
        }
    }

    [HttpPut("ChangePassword")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordRequest changePasswordRequest)
    {
        var jwt = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        string username = ReadJwt.ReadUsernameFromSubInJWTToken(HttpContext);

        if (changePasswordRequest == null)
        {
            Logger.LogMessage(username + ": Error changing password - It is null");
            return BadRequest("Invalid request");
        }


        if (string.IsNullOrEmpty(jwt))
        {
            Logger.LogMessage(username + ": Error changing password - JWT token is missing");
            return Unauthorized("JWT token is missing");
        }

        var serviceResult = await _IuserService.ChangeUserPassword(jwt, changePasswordRequest);

        if (serviceResult is OkResult)
        {
            Logger.LogMessage(username + ": Password changed successfully");
            return Ok("Password changed successfully");
        }
        return serviceResult;
    }
}