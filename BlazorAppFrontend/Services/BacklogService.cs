using System.Net.Http.Headers;
using BlazorAppTEST.Services.Auth;
using BlazorAppTEST.Services.Interface;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;


namespace BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using System.Net.Http;


public class BacklogService : IBacklogService
{

    //HTTPClient
    private readonly HttpClient httpClient;

    public BacklogService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserService.Jwt);
    }

    
    public async Task<IActionResult> CreateBacklogEntry(AddBacklogEntryRequest backlogEntry)
    {
        if (String.IsNullOrEmpty(backlogEntry.ProjectID))
        {
            throw new Exception("ProjectID is null or empty");
        }
        
        if (String.IsNullOrEmpty(backlogEntry.RequirmentNr))
        {
            throw new Exception("RequirementNr must be set");
        }
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/BBacklog/CreateBroker", backlogEntry);

        if (response.IsSuccessStatusCode)
        {
            return new OkResult(); 
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Error creating backlog entry: {response.StatusCode}, Details: {errorContent}");
        }
    }
}