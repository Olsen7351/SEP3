using BlazorAppTEST.Services.Auth;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
namespace BlazorAppTEST.Services;
using ClassLibrary_SEP3;
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



    public async Task<LogBookEntryPoints> GetLogBookEntryByID(string entryID)
    {
        if (string.IsNullOrEmpty(entryID))
        {
            throw new ArgumentException("EntryID is empty or null.");
        }

        HttpResponseMessage response = await httpClient.GetAsync($"api/LogBookEntries/{entryID}");

        if (response.IsSuccessStatusCode)
        {
            // Directly deserialize the JSON response into a LogBookEntryPoints object
            var entry = await response.Content.ReadFromJsonAsync<LogBookEntryPoints>();

            if (entry != null)
            {
                return entry; // Return the deserialized object
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

        HttpResponseMessage response = await httpClient.GetAsync($"api/LogBook/{projectID}");

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
            // You can handle different status codes differently if needed
            throw new HttpRequestException($"Error retrieving logbook entries: {response.ReasonPhrase}");
        }
    }


    public async Task<IActionResult> UpdateEntry(String EntryID, String Description)
    {
        if (String.IsNullOrEmpty(EntryID))
        {
            throw new Exception("EntryID was unable to be retrieved");
        }

        var payload = new { EntryID = EntryID, Description = Description };
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/UpdateEntry", payload);
        
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