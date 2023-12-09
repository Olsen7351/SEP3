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
}