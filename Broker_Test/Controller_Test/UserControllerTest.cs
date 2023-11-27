using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Broker_Test.Controller_Test;

public class UserControllerTest
{
    

    [Fact]
    public async Task CreateUser_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        var validUserRequest = new CreateUserRequest
        {
            username = "Alma",
            password = "Treats"
        };

        var expectedUserData = new User { Username = validUserRequest.username };
        
        mockUserService.Setup(service => service.CreateUser(validUserRequest))
            .ReturnsAsync(new CreatedAtActionResult("GetUser", "User", new { username = validUserRequest.username }, expectedUserData));

        
        var result = await controller.CreateUser(validUserRequest);
        
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtResult.StatusCode);
        var resultUser = Assert.IsType<User>(createdAtResult.Value);
        Assert.Equal(expectedUserData.Username, resultUser.Username);
    }

    [Fact]
    public async Task GetUserReturnOk()
    {
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        var username = "Alma";
        var expectedUser = new User { Username = username };

        mockUserService.Setup(service => service.GetUser(username)).ReturnsAsync(new OkObjectResult(expectedUser));
        var result = await controller.GetUser(username);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(expectedUser.Username, resultUser.Username);
        Assert.Equal(expectedUser.Password, resultUser.Password);

    }
    


}