using Broker.Controllers;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

    public async Task<IActionResult> ChangeUserPassword(string jwt, ChangePasswordRequest changePasswordRequest)
    {
        string requestUri = "api/users/change-password";
        var changePasswordDto = new ChangePasswordRequest
        {
            CurrentPassword = changePasswordRequest.CurrentPassword,
            NewPassword = changePasswordRequest.NewPassword
        };

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await httpClient.PutAsJsonAsync(requestUri, changePasswordDto);

        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }
        else
        {
            // Return msg based on status code
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return new BadRequestResult();
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedResult();
                default:
                    return new BadRequestResult();
            }
        }
    }
}