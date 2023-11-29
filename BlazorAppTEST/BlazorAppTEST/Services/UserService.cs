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
        if (String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.Password))
        {
            throw new Exception("One or many forms is empty, please fill them out before creating a new user");
        }

        if (user.Username.Length > 16)
        {
            throw new Exception("Username cant exceed 16 characters");
        }
        
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/User/CreateUser", user);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to create user: {error}");
        }
    }

    
    public async Task<User> Login(User user)
    {
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync("api/Login", user);

        //Guards
        if (String.IsNullOrEmpty(user.Username) || String.IsNullOrEmpty(user.Password))
        {
            throw new Exception("One or many forms is empty, please fill them out before logging in");
        }
        
        if (user.Username.Length > 16)
        {
            throw new Exception("Usernames can only be up to 16 characters long");
        }
        
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