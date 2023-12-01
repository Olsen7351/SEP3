using Broker.Controllers;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public class UserService : IUserService
{
    private readonly HttpClient httpClient;

    public UserService(HttpClient client)
    {
        this.httpClient = client;
        this.httpClient.BaseAddress = new Uri("http://localhost:8080/");
    }


    public async Task<IActionResult> CreateUser(CreateUserRequest user)
    {
        string requestUri = "api/users";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, user);
        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }
        
        return new BadRequestResult();
    }




    public async Task<IActionResult> LoginWithUserCredentials(User user)
    {
        string requestUri = "api/users/authenticate";
        var response = await httpClient.PostAsJsonAsync(requestUri, user);

        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return new OkObjectResult(token);
        }
        else
        {
            return new BadRequestResult();
        }
    }
}