
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
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;
using Task = System.Threading.Tasks.Task;


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
        public async void AddTaskToBackLog()
        {

            string projectId = "1";
            var taskToAdd = new AddBacklogTaskRequest()
            {
                ProjectId = projectId,
                Title = "Test Task",
                Status = TaskStatus.ToDo

            };
            string responseTaskJson = JsonSerializer.Serialize(taskToAdd);
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseTaskJson, Encoding.UTF8, "application/json")
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().EndsWith($"api/Project/{projectId}/Backlog/Task")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var result = await _backlogService.AddTaskToBackLog(projectId, taskToAdd);
            var returnedTask = Assert.IsType<ClassLibrary_SEP3.Task>(result);
            Assert.NotNull(returnedTask);
            Assert.Equal(taskToAdd.Title, returnedTask.Title);
            Assert.Equal(taskToAdd.Description, returnedTask.Description);
        }

        [Fact]
        public async void DeleteTaskFromBacklog_ValidRequest()
        {
            string ProjectId = "1";
            string Id = "1";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete && req.RequestUri.ToString()
                            .EndsWith($"api/Project/{ProjectId}/Backlog/Task/{Id}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var result = await _backlogService.DeleteTaskFromBacklog(Id, ProjectId);

            var okResult = Assert.IsType<OkResult>(result);
            Assert.NotNull(okResult);
        }

        [Fact]
        public async void DeleteTaskFromBacklogInvalidRequest()
        {
            string ProjectId = "1";
            string Id = "1";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Delete && req.RequestUri.ToString()
                            .EndsWith($"api/Project/{ProjectId}/Backlog/Task/{Id}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var result = await _backlogService.DeleteTaskFromBacklog(Id, ProjectId);

            var okResult = Assert.IsType<BadRequestResult>(result);
            Assert.NotNull(okResult);
        }
    }

}