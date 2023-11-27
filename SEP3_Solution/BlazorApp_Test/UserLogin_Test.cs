using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class UserLogin_Test
{
    [Fact]
    public async Task LoginWithRightInformation()
    {
        var httpClient = new HttpClient();
        var userService = new UserService(httpClient);

        var user = new User
        {
            Username = "TestUser",
            Password = "TestPassword"
        };
        
       // var loggedInUser = await userService.Login(user);
        
       // Assert.NotNull(loggedInUser);
       // Assert.Equal("TestUser", loggedInUser.Username);
    }
}