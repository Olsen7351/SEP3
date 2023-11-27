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

    public async Task<User> Login(User user)
    {
        HttpResponseMessage responseMessage = await httpClient.PostAsJsonAsync("api/Login", user);

        if (responseMessage.IsSuccessStatusCode)
        {
            // Use ReadAsAsync<T> to deserialize JSON content
            return await responseMessage.Content.ReadFromJsonAsync<User>();
        }
        else
        {
            // Handle the failure case, for example, return null or throw an exception
            return null;
        }
    }
}