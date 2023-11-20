using ClassLibrary_SEP3;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class UserService

{
    [Fact]
    public async Task CreateUser()
    {
        var user = new User
        {
            Username = "Test1",
            Password = "hetasjdbaj1437"
        };

       
    }
}
