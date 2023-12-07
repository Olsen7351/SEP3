using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public class LogBookService : ILogBookService
{
    private readonly HttpClient httpClient;

    public LogBookService(HttpClient client)
    {
        this.httpClient = client;
    }
    
    
    // Get
    public async Task<LogBook> GetEntriesForLogBook(string projectID)
    {
        string requestUri = $"api/LogBook/GetEntriesForLogBook?ProjectID={Uri.EscapeDataString(projectID)}";
        HttpResponseMessage response = await httpClient.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            var logBook = await response.Content.ReadFromJsonAsync<LogBook>();
            if (logBook != null)
            {
                return logBook;
            }
            else
            {
                throw new KeyNotFoundException("Logbook not found.");
            }
        }
        else
        {
            throw new HttpRequestException($"Error retrieving logbook entries: {response.ReasonPhrase}");
        }
    }

    public async Task<LogBookEntryPoints> GetSpecificEntry(string ProjectID, string EntryID)
    {
        if (string.IsNullOrEmpty(EntryID) || string.IsNullOrEmpty(ProjectID))
        {
            throw new ArgumentException("EntryID and ProjectID must not be null or empty.");
        }
        string requestUri = $"api/LogBook/{Uri.EscapeDataString(ProjectID)}/logbookentries/{Uri.EscapeDataString(EntryID)}";
        HttpResponseMessage response = await httpClient.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            var entry = await response.Content.ReadFromJsonAsync<LogBookEntryPoints>();
            if (entry != null)
            {
                return entry;
            }
            else
            {
                throw new KeyNotFoundException("Specific logbook entry not found.");
            }
        }
        else
        {
            throw new HttpRequestException($"Error retrieving the specific logbook entry: {response.ReasonPhrase}");
        }
    }

    public async Task<IActionResult> UpdateEntry(UpdateEntryRequest updateEntryRequest)
    {
        if (String.IsNullOrEmpty(updateEntryRequest.ProjectID) || String.IsNullOrEmpty(updateEntryRequest.EntryID))
        {
            throw new Exception("payload has entryID or projectID set to empty or null");
        }
        HttpResponseMessage response = await httpClient.PutAsJsonAsync("api/LogBook/UpdateEntry", updateEntryRequest);

        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }
        else
        {
            // Here, you might want to handle different types of error responses differently
            throw new Exception($"Error updating entry: {response.ReasonPhrase}");
        }
    }



    //Create
    public async Task<IActionResult> CreateNewEntryLogBook(AddEntryPointRequest logBookEntryPoints)
    {
        string requestUri = "api/LogBook/CreateLogEntryMicro";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, logBookEntryPoints);
        if (response.IsSuccessStatusCode)
        {
            var CreateNewEntryLogBook = await response.Content.ReadFromJsonAsync<LogBookEntryPoints>();
            return new OkObjectResult(CreateNewEntryLogBook);
        }
        
        return new BadRequestResult();
    }
}