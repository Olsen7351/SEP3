using Amazon.Util;
using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Broker_Test.Controller_Test;

public class User_ControllerTest
{
    /*
    [Fact]
    public async void CreateUser_ReturnsOk_WhenUserIsNotNull()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        var user = new User(); // Replace with valid user data

        // Mock the UserService to return an OkObjectResult for a successful user creation
        mockUserService.Setup(service => service.CreateUser(user))
            .ReturnsAsync(new OkObjectResult("User created successfully"));

        // Act
        var result = await controller.CreateUser(user);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("User created successfully", okResult.Value);
    }

    [Fact]
    public async void CreateUser_ReturnsBadRequest_WhenUserIsNull()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);
        User nullUser = null; // User is null

        // Act
        var result = await controller.CreateUser(nullUser);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }
    */
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

        // Simulate the [Required] validation for Password
        controller.ModelState.AddModelError("Password", "Password is required");

        User user = new User()
        {
            Username = "Hey",
            Password = null // Password is null
        };

        // Act
        var result = await controller.LoginWithUserCredentials(user);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        var modelStateDictionary = Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        Assert.True(modelStateDictionary.ContainsKey("Password"));
        var passwordErrors = modelStateDictionary["Password"] as string[];
        Assert.NotNull(passwordErrors);
        Assert.Contains("Password is required", passwordErrors);
    }




    
    
    
    [Fact]
    public async Task LoginNullUsername()
    {
        // Arrange
        var mockUserService = new Mock<IUserService>();
        var controller = new UserController(mockUserService.Object);

        // Simulate the [Required] validation for Username
        controller.ModelState.AddModelError("Username", "Username is required");

        User user = new User()
        {
            Username = null, // Username is null
            Password = "SomePassword" // Password is valid
        };

        // Act
        var result = await controller.LoginWithUserCredentials(user);

        // Assert
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        var modelStateDictionary = Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        Assert.True(modelStateDictionary.ContainsKey("Username"));
        var usernameErrors = modelStateDictionary["Username"] as string[];
        Assert.NotNull(usernameErrors);
        Assert.Contains("Username is required", usernameErrors);
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
        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        var modelStateDictionary = Assert.IsType<SerializableError>(badRequestObjectResult.Value);
        Assert.True(modelStateDictionary.ContainsKey("Username"));
        var usernameErrors = modelStateDictionary["Username"] as string[];
        Assert.NotNull(usernameErrors);
        Assert.Contains("Username is too long, only 16 characters are allowed", usernameErrors);
    }
}