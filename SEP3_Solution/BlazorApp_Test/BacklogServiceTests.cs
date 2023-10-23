using System.Net;
using Broker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ProjectMicroservice.Models;
using Task = System.Threading.Tasks.Task;
using BacklogService = MainWeb.Services.BacklogService;

namespace BlazorAppTest;

public class BacklogServiceTests
{
    [Fact]
    public async Task CreateBacklog_Should_SendHttpRequestAndHandleResponse()
    {
        // Arrange
        using var testServer = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>()); // Assuming you have a Startup class

        var client = testServer.CreateClient();
        var backlogService = new BacklogService(client);

        var backlog = new Backlog
        {
            Id = 1,
            ProjectId = 100,
            Description = "Sample Description"
        };

        // Act
        await backlogService.CreateBacklog(backlog);

        // Assert
        // You can assert based on the response you expect from the test server
        var response = await client.GetAsync("/api/Backlog"); // This depends on your test server setup
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        
    }
}

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure services relevant to your tests
        services.AddTransient<BacklogService>();
        
    }
}