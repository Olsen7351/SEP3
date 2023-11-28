using System.Net;
using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class UserLogin_Test
{
    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
    private readonly HttpClient _httpClient;
    private readonly IUserService _userService; // UserService also implements IUserLogin

    public UserLogin_Test()
    {
        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        // Mock setup black magic
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new User { Username = "TestUser" }))
            })
            .Verifiable();

        var httpClientHandler = _mockHttpMessageHandler.Object;
        _httpClient = new HttpClient(httpClientHandler)
        {
            BaseAddress = new Uri("http://localhost")
        };
        _userService = new UserService(_httpClient);
    }



    //---------------------------------------------------------------------Username
    [Fact]
    public async Task LoginWithRightInformation()
    {
        // Arrange
        var user = new User
        {
            Username = "TestUser",
            Password = "TestPassword"
        };

        // Act
        var loggedInUser = await ((IUserLogin)_userService).Login(user);

        // Assert
        Assert.NotNull(loggedInUser);
        Assert.Equal("TestUser", loggedInUser.Username);

        // Verify that the handler was called
        _mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Exactly(1), // We expect a single external request
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>()
        );
    }
    
  
    [Fact]
    public async Task LoginWithNullUsername()
    {
        // Arrange
        var user = new User
        {
            Username = null, 
            Password = "TestPassword"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => ((IUserLogin)_userService).Login(user));
        Assert.Equal("One or many forms is empty, please fill them out before logging in", exception.Message);
    }
    
    [Fact]
    public async Task LoginWithEmptyUsername()
    {
        // Arrange
        var user = new User
        {
            Username = "", 
            Password = "TestPassword"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => ((IUserLogin)_userService).Login(user));
        Assert.Equal("One or many forms is empty, please fill them out before logging in", exception.Message);
    }
    
    
    [Fact]
    public async Task LoginWithlongUsername()
    {
        // Arrange
        var user = new User
        {
            Username = "IAmOver16CharactersLong", 
            Password = "TestPassword"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => ((IUserLogin)_userService).Login(user));
        Assert.Equal("Usernames can only be up to 16 characters long", exception.Message);
    }
    
    
    //---------------------------------------------------------------------Password
    
    [Fact]
    public async Task LoginWithNullPassword()
    {
        // Arrange
        var user = new User
        {
            Username = "James", 
            Password = null
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => ((IUserLogin)_userService).Login(user));
        Assert.Equal("One or many forms is empty, please fill them out before logging in", exception.Message);
    }
    
    
    [Fact]
    public async Task LoginWithEmptyPassword()
    {
        // Arrange
        var user = new User
        {
            Username = "James", 
            Password = ""
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => ((IUserLogin)_userService).Login(user));
        Assert.Equal("One or many forms is empty, please fill them out before logging in", exception.Message);
    }
}
