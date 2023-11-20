using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class UserLoginTest
{
    [Fact]
    public async Task LoginWithRightLoginInformation()
    {
        // Arrange
        var userService = new UserService();
        var user = new User
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        await userService.CreateUser(user);

        // Act
        var loggedInUser = userService.Login("TestUser", "TestPassword");

        // Assert
        Assert.NotNull(loggedInUser);
        Assert.Equal("TestUser", loggedInUser.Username);
    }

    
    
    [Fact]
    public void LoginWithWrongPassword()
    {
        // Arrange
        var userService = new UserService();
        var user = new User
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        userService.CreateUser(user).Wait();

        // Act
        var loggedInUser = userService.Login("TestUser", "WrongPassword");

        // Assert
        Assert.Null(loggedInUser);
    }
}