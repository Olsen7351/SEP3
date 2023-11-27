using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class UserLogin_Test
{
    private readonly Mock<IUserLogin> _mockUserService;
    private IUserLogin _userService;
    
    public UserLogin_Test()
    {
        _mockUserService = new Mock<IUserLogin>();
        _userService = _mockUserService.Object;
    }
    [Fact]
    public async Task LoginWithRightInformation_ReturnsLoggedInUser()
    {
        // Arrange
        var mockHttpClient = new Mock<HttpClient>();
        var userService = new UserService(mockHttpClient.Object);

        var user = new User
        {
            Username = "TestUser",
            Password = "TestPassword"
        };

        var loggedInUser = new User
        {
            Username = "TestUser",
            // Set other properties as needed
        };

        _mockUserService.Setup(service => service.Login(user))
            .ReturnsAsync(loggedInUser);

        // Act
        var result = await _userService.Login(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestUser", result.Username);
    }

    [Fact]
    public async Task LoginWithWrongInformation()
    {
        var mockHttpClient = new Mock<HttpClient>();
        var userService = new UserService(mockHttpClient.Object);

        var user = new User
        {
            Username = "WrongUser",
            Password = "WrongPassword"
        };

        _mockUserService.Setup(service => service.Login(user))
            .ReturnsAsync((User)null);

        // Act
        var result = await _userService.Login(user);

        // Assert
        Assert.Null(result);
    }
    
}



