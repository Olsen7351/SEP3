using BlazorAppTest;
using ClassLibrary_SEP3;
using Xunit;
using Task = System.Threading.Tasks.Task;

public class CreateUserTest
{
    
    //Create user test
    [Fact]
    public async Task CreateUser()
    {
        // Arrange
        var userService = new UserServiceHelper();
        var user = new User
        {
            Username = "Test1",
            Password = "hetasjdbaj1437"
        };

        // Act
        await userService.CreateUser(user);

        // Assert
        Assert.True(userService.UserExists("Test1"));
        Assert.True(userService.passwordExists("hetasjdbaj1437"));
    }
    
    
    [Fact]
    public async Task CreatingUserWithLongUsername()
    {
        // Arrange
        var userService = new UserServiceHelper();
        var user = new User
        {
            Username = new string('a', 17),
            Password = "password123"
        };

        
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await userService.CreateUser(user));
        Assert.Contains("Username is to long, only 16 characters is allowed", exception.Message);
    }
}
