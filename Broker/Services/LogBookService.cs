using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public class LogBookService : ILogBookService
{
    private readonly HttpClient httpClient;

    public LogBookService(HttpClient client)
    {
        this.httpClient = client;
    }
    
    
    public async Task<IActionResult> CreateNewEntryLogBook(LogBookEntryPoints logBookEntryPoints)
    {
        string requestUri = "api/CreateNewEntryLogBook";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, logBookEntryPoints);
        if (response.IsSuccessStatusCode)
        {
            var CreateNewEntryLogBook = await response.Content.ReadFromJsonAsync<LogBookEntryPoints>();
            return new OkObjectResult(CreateNewEntryLogBook);
        }
        
        return new BadRequestResult();
    }
}