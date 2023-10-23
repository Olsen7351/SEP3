using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;
using Task = System.Threading.Tasks.Task;

namespace Broker.Services;

public class BacklogService :IBacklogService
{
    private readonly HttpClient httpClient;

    public BacklogService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<ActionResult> CreateAsync(Backlog backlog)
    {
        if (backlog == null)
        {
            return new BadRequestResult();
        }

        HttpResponseMessage response = await httpClient.PostAsJsonAsync("api/Project", backlog);
        if (response.IsSuccessStatusCode)
        {
            return new OkResult();
        }
        else
        {
            return new BadRequestResult();
        }
    }

  
}