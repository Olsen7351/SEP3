using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Services;

namespace ProjectMicroservice.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _iUserService;

    public UserController(IUserService iUserService)
    {
        _iUserService = iUserService;
    }

    [HttpPost("CreateUser")]
    public IActionResult CreateUser([FromBody] User user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Returns a 400 Bad Request with validation errors.
        }

        if (user.Username.Length > 16)
        {
            ModelState.AddModelError("Username", "Username exceeds 16 characters");
            return BadRequest(ModelState);
        }

        var createdUser = _iUserService.CreateUser(user);
        return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Username }, createdUser);
    }
    
    
    
    
    [HttpPost]
    [Route("Login")]
    public IActionResult Login([FromBody] User loginRequest)
    {
        try
        {
            var token = _iUserService.Login(loginRequest);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}