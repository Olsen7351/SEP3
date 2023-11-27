using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class UserLogin_Test
{
    private Mock<IUserLogin> _mockService;
    private UserService _service;
    
    public UserLogin_Test()
    {
        _mockService = new Mock<IUserLogin>();
        _service = _mockService;
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

        _mockService.Setup(service => service.Login(user))
            .ReturnsAsync(loggedInUser);

        // Act
        var result = await _service.Login(user);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("TestUser", result.Username);
    }
}
}

