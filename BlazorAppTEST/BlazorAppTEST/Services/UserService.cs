using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public class UserService : IUserService, IUserLogin 
{
    //HTTPClient
    private readonly HttpClient httpClient;


    public UserService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task createUser(User user)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/CreateUser", user);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to create user");
        }
    }
}