using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Broker_Test.Controller_Test;

public class User_ControllerTest
{

    [Fact]
    public async Task CreateUser_ReturnsOk_WhenRequestIsValid()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        var createUserRequest = new CreateUserRequest { Username = "TestUser", Password = "TestPassword" };

        mockUserService.Setup(service => service.CreateUser(createUserRequest))
            .ReturnsAsync(new OkObjectResult(new User { Username = createUserRequest.Username, Password = createUserRequest.Password }));

        // Act
        var result = await controller.CreateUser(createUserRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var userResult = Assert.IsType<User>(okResult.Value);        
        Assert.NotNull(userResult);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(userResult.Username, createUserRequest.Username);
        Assert.Equal(userResult.Password, createUserRequest.Password );
        
    }

    [Fact]
    public async void CreateUser_ReturnsBadRequest_WhenUserIsNull()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        CreateUserRequest nullUser = null; // User is null

        // Act
        var result = await controller.CreateUser(nullUser);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    
    // -------------------------------------------------------------------------------------------------Login
    [Fact]
    public async Task LoginWithUserCredentials()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        var user = new User
        {
            Username = "TestName1",
            Password = "TestPassword2"
        };

        // Mock the UserService to return an OkObjectResult for a successful login
        mockUserService.Setup(service => service.LoginWithUserCredentials(user))
            .ReturnsAsync(new OkObjectResult("Login successful"));

        // Act
        var result = await controller.LoginWithUserCredentials(user);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Login successful", okResult.Value);
    }
    
    [Fact]
    public async Task LoginNullUser()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        User nullUser = null; // User is null

        // Act
        var result = await controller.LoginWithUserCredentials(nullUser);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    
    [Fact]
    public async Task LoginNullUserPassword()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        

        User user = new User()
        {
            Username = "Hey",
            Password = null // Password is null
        };

        // Act
        var result = await controller.LoginWithUserCredentials(user);

        // Assert

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Password is required",badRequest.Value);
    }
    
    [Fact]
    public async Task LoginNullUsername()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        

        User user = new User()
        {
            Username = null, // Username is null
            Password = "SomePassword" // Password is valid
        };

        // Act
        var result = await controller.LoginWithUserCredentials(user);

        // Assert

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username is required",badRequest.Value);
    }
    [Fact]
    public async Task LoginWithTooLongUsername()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);

        // Create a username longer than 16 characters
        string longUsername = "IAm16CharactersLong";

        // Manually add the expected error to the ModelState
        controller.ModelState.AddModelError("Username", "Username is too long, only 16 characters are allowed");

        User user = new User()
        {
            Username = longUsername, // Username is too long
            Password = "SomePassword" // Password is valid
        };
        // Act
        var result = await controller.LoginWithUserCredentials(user);

        // Assert

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Username is too long, only 16 characters are allowed",badRequest.Value);
    }

    [Fact]
    public async Task ChangeUserPassword_ValidRequest_ReturnsOk()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        var changePasswordRequest = new ChangePasswordRequest
        {
            CurrentPassword = "OldPassword",
            NewPassword = "NewPassword"
        };

        mockUserService.Setup(service => service.ChangeUserPassword(It.IsAny<string>(), It.IsAny<ChangePasswordRequest>()))
            .ReturnsAsync(new OkResult());

        var mockHttpContext = new DefaultHttpContext();
        mockHttpContext.Request.Headers["Authorization"] = "Bearer testtoken";
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext
        };

        // Act
        var result = await controller.ChangeUserPassword(changePasswordRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<OkObjectResult>(okResult);
    }
    
    [Fact]
    public async Task ChangeUserPassword_NullRequest_ReturnsBadRequest()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);

        var mockHttpContext = new DefaultHttpContext();
        mockHttpContext.Request.Headers["Authorization"] = "Bearer testtoken";
        controller.ControllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext
        };

        // Act
        var result = await controller.ChangeUserPassword(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        
    }
}