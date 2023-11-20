using ClassLibrary_SEP3.DataTransferObjects;

namespace Broker_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ProjectMicroservice.DataTransferObjects;
using Task = System.Threading.Tasks.Task;


public class SprintServiceTest
{
    private readonly HttpClient _httpClient;
    private readonly Mock<HttpMessageHandler> messageHandler;
    private readonly SprintService _sprintService;

    
    public SprintServiceTest()
    {
        messageHandler = new Mock<HttpMessageHandler>();
        _httpClient = new HttpClient(messageHandler.Object)
        {
            BaseAddress = new Uri("Http://mockserver/")
        };
        _sprintService = new SprintService(_httpClient);
    }

    
    [Fact]
    public async Task CreateSprintBackLog()
    {
        var sprintbacklog = new SprintBackLogRequest
        {
            SprintBacklog_ProjectID = "1",
            SprintBacklog_Id = "1",
            SprintBacklog_Title = "Ninis sprintbacklog",
            SprintBacklog_Tasks = new List<ClassLibrary_SEP3.Task>()
        };
        string responseTaskJson = JsonSerializer.Serialize(sprintbacklog);
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
            Content = new StringContent(responseTaskJson, Encoding.UTF8, "application/json")
        };
        messageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri.ToString().EndsWith($"api/Project/{sprintbacklog.SprintBacklog_ProjectID}/SprintTasks/{sprintbacklog.SprintBacklog_Id}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var result = await _sprintService.CreateSprintBacklog(sprintbacklog);
        var returnedSprint = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(returnedSprint);
        
    }

    [Fact]
    public async Task getSprintBackLogs()
    {
        var sprintbacklog = new GetSprintBacklogRequest()
        {
            SprintBacklog_ProjectID = "1",
            SprintBacklog_Id = "1"
        };
        string responseJson = JsonSerializer.Serialize(sprintbacklog);
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
        };
        messageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri.ToString().Contains($"api/Project/{sprintbacklog.SprintBacklog_ProjectID}/SprintTasks/")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        var result =
            await _sprintService.GetSprintBacklog(sprintbacklog.SprintBacklog_ProjectID,
                sprintbacklog.SprintBacklog_Id);
        Assert.NotNull(result);
        Assert.IsType<List<SprintBackLog>>(result);
    }
    
    
}