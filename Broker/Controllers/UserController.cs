using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.DataTransferObjects;
using Xunit.Sdk;

namespace DefaultNamespace;

[Route("api/")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest userRequest)
    {
        // TO Implement
        throw new NotImplementedException();
    }

    
    [HttpGet("{username")]
    public async Task<IActionResult> GetUser(string username)
    {
        throw new NotImplementedException();
    }
    
    
    
}