using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using ClassLibrary_SEP3.DataTransferObjects;

namespace Broker.Services;

public class BBacklogService : IBBacklogService
{
    private readonly HttpClient httpClient;
    
    public BBacklogService(HttpClient client)
    {
        this.httpClient = client;
    }

    public async Task<IActionResult> CreateBacklogEntry(AddBacklogEntryRequest backlogEntry)
    {
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/BBacklog/Create", backlogEntry);
        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error creating backlog entry: {response.StatusCode}, Details: {errorContent}");
        }
    }

    
    
    public async Task<BBackLog> GetBacklogForProject(string projectId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new Exception("ProjectID cant be null or empty");
        }
        
        HttpResponseMessage response = await httpClient.GetAsync($"api/BBacklog/GetBacklogMicro/{projectId}");
        if (response.IsSuccessStatusCode)
        {
            BBackLog backlog = await response.Content.ReadFromJsonAsync<BBackLog>();
            return backlog;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error retrieving backlog: {response.StatusCode}, Details: {errorContent}");
        }
    }

    public async Task<BacklogEntries> GetSpecificBacklogEntry(string projectId, string backlogEntryId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new ArgumentException("ProjectID can't be null or empty", nameof(projectId));
        }

        if (string.IsNullOrEmpty(backlogEntryId))
        {
            throw new ArgumentException("BacklogEntryID can't be null or empty", nameof(backlogEntryId));
        }
        
        HttpResponseMessage response = await httpClient.GetAsync($"api/BBacklog/GetSpecificBacklogEntryBroker?projectId={projectId}&backlogEntryId={backlogEntryId}");
        if (response.IsSuccessStatusCode)
        {
            BacklogEntries backlogEntry = await response.Content.ReadFromJsonAsync<BacklogEntries>();
            return backlogEntry;
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Error retrieving specific backlog entry: {response.StatusCode}, Details: {errorContent}");
        }
    }
}