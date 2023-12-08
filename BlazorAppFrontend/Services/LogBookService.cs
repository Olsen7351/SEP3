using BlazorAppTEST.Services.Auth;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
namespace BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

public class LogBookService : ILogBookService
{
    //HTTPClient
    private readonly HttpClient httpClient; 

    public LogBookService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserService.Jwt);
    }



    public async Task<LogBookEntryPoints> GetSpecificLogBookEntryByID(string entryID, string projectID)
    {
        if (string.IsNullOrEmpty(entryID))
        {
            throw new ArgumentException("EntryID is empty or null.");
        }

        if (string.IsNullOrEmpty(projectID))
        {
            throw new ArgumentException("ProjectID is empty or null.");
        }
        
        HttpResponseMessage response = await httpClient.GetAsync($"api/LogBook/{projectID}/logbookentries/{entryID}");
        if (response.IsSuccessStatusCode)
        {
            var entry = await response.Content.ReadFromJsonAsync<LogBookEntryPoints>();

            if (entry != null)
            {
                if(entry.ProjectID != projectID)
                {
                    throw new Exception("The logbook entry does not belong to the provided project.");
                }
                return entry;
            }
            else
            {
                throw new Exception("LogBook entry was not found.");
            }
        }
        else
        {
            throw new HttpRequestException($"Error retrieving logbook entry: {response.ReasonPhrase}");
        }
    }






    public async Task<LogBook> GetEntriesForLogBook(string projectID)
    {
        if (string.IsNullOrEmpty(projectID))
        {
            throw new ArgumentException("Project ID is empty or null.");
        }
        

        // Append the projectID to the URI path if that's how your API expects it
        HttpResponseMessage response = await httpClient.GetAsync($"api/LogBook/GetLogEntries?ProjectID={projectID}");
        var responseBody = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var logBook = await response.Content.ReadFromJsonAsync<LogBook>();

            if (logBook != null)
            {
                return logBook;
            }
            else
            {
                throw new Exception("LogBook data was not found.");
            }
        }
        else
        {
            throw new HttpRequestException($"Error retrieving logbook entries: {response.ReasonPhrase}");
        }
    }



    public async Task<IActionResult> UpdateEntry(UpdateEntryRequest updateEntryRequest)
    {
        if (String.IsNullOrEmpty(updateEntryRequest.EntryID))
        {
            throw new Exception("EntryID was unable to be retrieved");
        }
        HttpResponseMessage response = await httpClient.PutAsJsonAsync("api/LogBook/UpdateEntry", updateEntryRequest);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
        return new OkResult();
    }
    
    
    
    //Create Entries 
    public async Task<IActionResult> CreateNewEntryToLogBook (AddEntryPointRequest logBookEntryPoints)
    {
        if (String.IsNullOrEmpty(logBookEntryPoints.OwnerUsername) || logBookEntryPoints.CreatedTimeStamp == null)
        {
            throw new Exception("Either username hasn't been assigned or time stamp is null");
        }

        if (logBookEntryPoints.CreatedTimeStamp.Date != DateTime.UtcNow.Date)
        {
            throw new Exception("Created entry needs to have today's date");
        }

        if (String.IsNullOrEmpty(logBookEntryPoints.ProjectID))
        {
            throw new Exception("ProjectID is null or empty");
        }

        if (String.IsNullOrEmpty(logBookEntryPoints.Description))
        {
            throw new Exception("Description cant be empty");
        }
        
        
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/LogBook/CreateLogEntryBroker", logBookEntryPoints);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
        return new OkResult();
    }
}