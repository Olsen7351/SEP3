using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;
using Xunit.Sdk;

namespace Broker.Services;

public class BacklogService : IBacklogService
{
    private readonly HttpClient httpClient;

    public BacklogService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }


    public async Task<IActionResult> GetBacklog(int projectId)
    {
        if (projectId < 0)
        {
            throw new BadHttpRequestException("ProjectId must be greater than 0");
        }

        string requestUri = $"api/Backlog/{projectId}";
        HttpResponseMessage response = await httpClient.GetAsync(requestUri);

        if (response.IsSuccessStatusCode)
        {
            var backlog = await response.Content.ReadFromJsonAsync<Backlog>();

            if (backlog != null)
            {
                return new OkObjectResult(backlog);
            }
            else
            {
                return new NotFoundResult();
            }
        }
        else
        {
            return new BadRequestResult();
        }
    }

    public async Task<IActionResult> CreateBacklog(Backlog backlog)
    {
        string requestUri = $"api/Backlog";
        HttpResponseMessage response = await httpClient.PostAsJsonAsync(requestUri, backlog);
        if (response.IsSuccessStatusCode)
        {
            var backlogResponse = await response.Content.ReadFromJsonAsync<Backlog>();
            if (backlogResponse != null)
            {
                return new OkObjectResult(backlogResponse);
            }
            else
            {
                return new BadRequestResult();
            }
        }
        else
        {
            return new BadRequestResult();
        }
    }
}