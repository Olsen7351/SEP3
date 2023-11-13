
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using Broker.Services;
using System.Text;
using System.Text.Json;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using ClassLibrary_SEP3;

namespace Broker_Test
{
    public class BacklogServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly BacklogService _backlogService;
        public BacklogServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://mockserver/")
            };
            _backlogService = new BacklogService(_httpClient);
        }
        [Fact]
        public async Task AddTaskToBackLog()
        {

            int projectId = 1;
            var taskToAdd = new ClassLibrary_SEP3.Task
            {
                ProjectId = ObjectId.GenerateNewId(),
                Title = "Test Task",
                Description = "This is a test task",

            };
            string responseTaskJson = JsonSerializer.Serialize(taskToAdd);
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseTaskJson, Encoding.UTF8, "application/json")
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith($"api/Project/{projectId}/Backlog/Task")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var result = await _backlogService.AddTaskToBackLog(projectId, taskToAdd);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTask = Assert.IsType<ClassLibrary_SEP3.Task>(okResult.Value);
            Assert.NotNull(returnedTask);
            Assert.Equal(taskToAdd.Title, returnedTask.Title);
            Assert.Equal(taskToAdd.Description, returnedTask.Description);
        }

        [Fact]
        public async Task DeleteTaskFromBacklog_ValidRequest_ReturnsSuccess()
        {
            int projectId = 1;
            int backlogId = 2;
            var deleteTaskRequest = new DeleteBacklogTaskRequest
            {
                TaskId = ObjectId.GenerateNewId()
            };
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.ToString().EndsWith($"api/Project/{projectId}/Backlog/{backlogId}/Task/{deleteTaskRequest.TaskId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var result = await _backlogService.DeleteTaskFromBacklog(projectId, backlogId, deleteTaskRequest);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            Assert.IsType<HttpResponseMessage>(okResult.Value);
        }

    }

}