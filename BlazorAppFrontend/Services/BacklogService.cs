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


    public async Task<BBackLog> GetBacklogForProject(string projectIdAsString)
    {
        if (string.IsNullOrEmpty(projectIdAsString))
        {
            throw new ArgumentException("ProjectID can't be null or empty", nameof(projectIdAsString));
        }


        HttpResponseMessage response = await httpClient.GetAsync($"api/BBacklog/GetBacklogBroker/{projectIdAsString}");

        if (response.IsSuccessStatusCode)
        {
            var bbacklog = await response.Content.ReadFromJsonAsync<BBackLog>();
            if (bbacklog != null)
            {
                return bbacklog;
            }
            else
            {
                throw new InvalidOperationException("Backlog data is null.");
            }
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Error retrieving backlog entry: {response.StatusCode}, Details: {errorContent}");
        }
    }


    
    public async Task<BacklogEntries> GetBacklogEntryById(string projectId, string backlogEntryId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new ArgumentException("ProjectID can't be null or empty", nameof(projectId));
        }

        if (string.IsNullOrEmpty(backlogEntryId))
        {
            throw new ArgumentException("BacklogEntryID can't be null or empty", nameof(backlogEntryId));
        }

        HttpResponseMessage response = await httpClient.GetAsync($"api/BBacklog/{projectId}/backlogentries/{backlogEntryId}");
        if (response.IsSuccessStatusCode)
        {
            var backlogEntry = await response.Content.ReadFromJsonAsync<BacklogEntries>();
            if (backlogEntry != null)
            {
                return backlogEntry;
            }
            else
            {
                throw new InvalidOperationException("Backlog entry not found.");
            }
        }
        else
        {
            throw new HttpRequestException(
                $"Error retrieving backlog entry: {response.StatusCode}, Details: {response.ReasonPhrase}");
        }
    }

    public async Task<IActionResult> UpdateBacklogEntry(UpdateBacklogEntryRequest updateRequest)
    {
        if (string.IsNullOrEmpty(updateRequest.ProjectID))
        {
            throw new Exception("ProjectID can't be null or empty");
        }

        if (string.IsNullOrEmpty(updateRequest.EntryID))
        {
            throw new Exception("EntryID can't be null or empty");
        }
        HttpResponseMessage response = await httpClient.PutAsJsonAsync($"api/BBacklog/UpdateEntry", updateRequest);

        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"Error updating backlog entry: {response.StatusCode}, Details: {errorContent}");
        }
    }
}