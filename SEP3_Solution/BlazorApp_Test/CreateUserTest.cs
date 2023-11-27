using System.ComponentModel.DataAnnotations;
using BlazorAppTest;
using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Moq;
using Xunit;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel.DataAnnotations;
using Xunit;
using ClassLibrary_SEP3;


public class CreateUserTest
{
    private readonly Mock<IUserService> _mockUserService;
    private IUserService _userService;


    public CreateUserTest()
    {
        _mockUserService = new Mock<IUserService>();
        _userService = _mockUserService.Object;
    }

    private void ValidateModel(User user)
    {
        var context = new ValidationContext(user, serviceProvider: null, items: null);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(user, context, results, true);
        if (!isValid)
        {
            var compositeErrors = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new ValidationException(compositeErrors);
        }
    }


    //-------------------------------------------------------------------------------------------------------------------------------------------------------- User
    [Fact]
    public async Task CreateUser()
    {
        // Arrange
        var user = new User
        {
            Username = "Test1",
            Password = "hetasjdbaj1437"
        };

        _mockUserService.Setup(service => service.createUser(user))
            .Returns(Task.CompletedTask);

        // Act
        var exception = await Record.ExceptionAsync(() => _userService.createUser(user));

        // Assert
        Assert.Null(exception);
    }


    [Fact]
    public async Task CreatingUserWithLongUsername()
    {
        // Arrange
        var user = new User
        {
            Username = "imimimimimimimimimimimimimimimimimimim",
            Password = "password123"
        };

        _mockUserService.Setup(service => service.createUser(It.IsAny<User>()))
            .ThrowsAsync(new ArgumentException("Username is too long, only 16 characters are allowed"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _userService.createUser(user));

        Assert.Equal("Username is too long, only 16 characters are allowed", exception.Message);
    }


    [Fact]
    public async Task CreateUserWithNullUsername()
    {
        // Arrange
        var userWithNullUsername = new User
        {
            Username = null, // Invalid data
            Password = "ValidPassword"
        };

        // Setup the mock to throw a ValidationException for invalid user data
        _mockUserService.Setup(service => service.createUser(It.Is<User>(u => u.Username == null)))
            .ThrowsAsync(new ValidationException("Username is required"));

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => _userService.createUser(userWithNullUsername));
        Assert.Contains("Username is required", exception.Message);
    }

    [Fact]
    public async Task CreateUserWithEmptyUsername()
    {
        // Arrange
        var userWithEmptyUsername = new User
        {
            Username = "", 
            Password = "123456"
        };

        // Setup the mock to throw a ValidationException for invalid user data
        _mockUserService.Setup(service => service.createUser(It.Is<User>(u => u.Username == "")))
            .ThrowsAsync(new ValidationException("Username is required"));

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => _userService.createUser(userWithEmptyUsername));
        Assert.Contains("Username is required", exception.Message);
    }


    // -------------------------------------------------------------------------------------------------------------------------------------------------------- Password
    [Fact]
    public async Task CreateUserWithNullPassword()
    {
        // Arrange
        var userWithNullPassword = new User
        {
            Username = "123456",
            Password = null 
        };

        // Setup the mock to throw a ValidationException for invalid user data
        _mockUserService.Setup(service => service.createUser(It.Is<User>(u => u.Password == null)))
            .ThrowsAsync(new ValidationException("Password is required"));

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => _userService.createUser(userWithNullPassword));
        Assert.Contains("Password is required", exception.Message);
    }

    
    
    [Fact]
    public async Task CreateUserWithEmptyPassword()
    {
        // Arrange
        var userWithEmptyPassword = new User
        {
            Username = "ValidUsername",
            Password = "" 
        };

        // Setup the mock to throw a ValidationException for invalid user data
        _mockUserService.Setup(service => service.createUser(It.Is<User>(u => u.Password == "")))
            .ThrowsAsync(new ValidationException("Password is required"));

        // Act & Assert
        var exception =
            await Assert.ThrowsAsync<ValidationException>(() => _userService.createUser(userWithEmptyPassword));
        Assert.Contains("Password is required", exception.Message);
    }
}