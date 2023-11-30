using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services;
using ClassLibrary_SEP3;

public class LogBookService : ILogBookService
{
    //HTTPClient
    private readonly HttpClient httpClient; 

    public LogBookService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    
    
    
    
    public async Task<IActionResult> GetEntriesForLogBook(String projectID)
    {
     
        if ( String.IsNullOrEmpty(projectID))
        {
            throw new Exception("Either ProjectId is empty or null");
        }
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/GetLogEntries", projectID);
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
        return new OkResult();
    }
    
    
    
    
    
    
    
    
    //Create Entries 
    public async Task<IActionResult> CreateNewEntryToLogBook (LogBookEntryPoints logBookEntryPoints)
    {
        if (String.IsNullOrEmpty(logBookEntryPoints.OwnerUsername) || logBookEntryPoints.CreatedTimeStamp == null)
        {
            throw new Exception("Either username hasn't been assigned or time stamp is null");
        }

        if (logBookEntryPoints.CreatedTimeStamp > DateTime.Today)
        {
            throw new Exception("Created entry needs to have a present timestamp");
        }

        if (String.IsNullOrEmpty(logBookEntryPoints.Description))
        {
            throw new Exception("Description cant be empty");
        }
        
        //Ask for help
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/CreateLogEntry", logBookEntryPoints);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error:{response.StatusCode}");
        }
        return new OkResult();
    }
}