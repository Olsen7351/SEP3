using System.ComponentModel.DataAnnotations;
using BlazorAppTest;
using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel.DataAnnotations;
using System.Net;
using BlazorAppTEST.Services.Auth;
using Xunit;
using ClassLibrary_SEP3;
using Moq.Protected;

namespace BlazorAppTest;
public class CreateUser_Test
{
    private readonly Mock<IUserService> _mockUserService;
    private IUserService _userService;


    public CreateUser_Test()
    {
        _mockUserService = new Mock<IUserService>();
        _userService = _mockUserService.Object;
    }

    

    //--------------------------------------------------------------------------------------------------------------- User
    [Fact]
    public async Task CreateUser()
    {
        // Arrange
        var user = new User
        {
            Username = "Test1",
            Password = "sjhbafjhasbf"
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var userService = new UserService(httpClient);

        
        // Act
        Func<Task> act = async () => await userService.createUser(user);

        // Assert
        await act(); 
    }


    [Fact]
    public async Task CreatingUserWithLongUsername()
    {
        // Arrange
        var userWithNullUsername = new User
        {
            Username = "IAmOver16CharactersLong",
            Password = "ValidPassword"
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var userService = new UserService(httpClient);

        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => userService.createUser(userWithNullUsername));
        Assert.Contains("Username cant exceed 16 characters", exception.Message);
    }


    [Fact]
    public async Task CreateUserWithNullUsername()
    {
        // Arrange
        var userWithNullUsername = new User
        {
            Username = null, // Null username should trigger exception
            Password = "ValidPassword"
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var userService = new UserService(httpClient);

        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => userService.createUser(userWithNullUsername));
        Assert.Contains("One or many forms is empty, please fill them out before creating a new user", exception.Message);
    }


    
    
    
    [Fact]
    public async Task CreateUserWithEmptyUsername()
    {
        // Arrange
        var userWithNullUsername = new User
        {
            Username = "", 
            Password = "ValidPassword"
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var userService = new UserService(httpClient);

        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => userService.createUser(userWithNullUsername));
        Assert.Contains("One or many forms is empty, please fill them out before creating a new user", exception.Message);
    }


    // -------------------------------------------------------------------------------------------------------- Password
    [Fact]
    public async Task CreateUserWithNullPassword()
    {
        // Arrange
        var userWithNullUsername = new User
        {
            Username = "James", 
            Password = null
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var userService = new UserService(httpClient);

        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => userService.createUser(userWithNullUsername));
        Assert.Contains("One or many forms is empty, please fill them out before creating a new user", exception.Message);
    }

    
    
    [Fact]
    public async Task CreateUserWithEmptyPassword()
    {
        // Arrange
        var userWithNullUsername = new User
        {
            Username = "James", 
            Password = ""
        };

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var userService = new UserService(httpClient);

        
        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => userService.createUser(userWithNullUsername));
        Assert.Contains("One or many forms is empty, please fill them out before creating a new user", exception.Message);
    }
}