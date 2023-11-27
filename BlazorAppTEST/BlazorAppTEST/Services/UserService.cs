using ClassLibrary_SEP3;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services;

public class UserService : IUserService, IUserLogin 
{
    //HTTPClient
    private readonly HttpClient httpClient;
    
    // Constructor to inject the HTTPClient
    public UserService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task createUser(User user)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/CreateUser", user);
    }
    
    //login

    public async Task<User> Login(User user)
    {
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync("api/Login", user);
        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadFromJsonAsync<User>();
        }
        else
        {
            return null;
        }
      
    }
}