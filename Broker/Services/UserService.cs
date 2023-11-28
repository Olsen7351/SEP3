using Broker.Controllers;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public class UserService : IUserService
{
    private readonly HttpClient httpClient;

    public UserService(HttpClient client)
    {
        this.httpClient = client;
    }


    public async Task<IActionResult> CreateUser(User user)
    {
        
        string requestUri = "api/CreateUser";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, user);
        if (response.IsSuccessStatusCode)
        {
            return new CreatedAtActionResult(nameof(UserController.CreateUser), "UserController",
                new { id = createdUser?.Id }, createdUser);
        }
        
        return new BadRequestResult();
    }

    
    

    public async Task<IActionResult> LoginWithUserCredentials(User user)
    {
        string requestUri = "api/Login";
        var response = await httpClient.PostAsJsonAsync(requestUri, user);

        if (response.IsSuccessStatusCode)
        {
            var loggedInUser = await response.Content.ReadFromJsonAsync<User>();
            return new OkObjectResult(loggedInUser);
        }
        else
        {
            return new BadRequestResult(); 
        }
    }
}