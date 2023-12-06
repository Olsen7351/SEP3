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
                // Ideally, you should throw a custom exception that your error handling middleware can catch to return a 404 status code.
                throw new KeyNotFoundException("Logbook not found.");
            }
        }
        else
        {
            // You should throw an exception here to be handled by your error handling middleware.
            throw new HttpRequestException($"Error retrieving logbook entries: {response.ReasonPhrase}");
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