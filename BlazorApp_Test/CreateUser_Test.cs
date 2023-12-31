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
using ClassLibrary_SEP3.DataTransferObjects;
using Moq.Protected;

namespace BlazorAppTest;
public class CreateUser_Test

{/*
    private readonly Mock<IUserLogin> _mockUserService;
    private IUserLogin _userService;


    public CreateUser_Test()
    {
        _mockUserService = new Mock<IUserLogin>();
        _userService = _mockUserService.Object;
    }

    

    //--------------------------------------------------------------------------------------------------------------- User
    [Fact]
    public async Task CreateUser()
    {
        // Arrange
        var user = new CreateUserRequest()
        {
            Username = "Test1",
            Password = "sjhbafjhasbf",
            Email = "test@gmail.com"
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
        var userWithNullUsername = new CreateUserRequest()
        {
            Username = "IAmOver16CharactersLong",
            Password = "ValidPassword",
            Email = "test@gmail.com"
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
        var userWithNullUsername = new CreateUserRequest()
        {
            Username = null, // Null username should trigger exception
            Password = "ValidPassword",
            Email = "test@gmail.com"
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
        var userWithNullUsername = new CreateUserRequest()
        {
            Username = "", 
            Password = "ValidPassword",
            Email = "test@gmail.com"
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
        var userWithNullUsername = new CreateUserRequest()
        {
            Username = "James", 
            Password = null,
            Email = "test@gmail.com"
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
        var userWithNullUsername = new CreateUserRequest()
        {
            Username = "James", 
            Password = "",
            Email = "test@gmail.com"
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
    */
}