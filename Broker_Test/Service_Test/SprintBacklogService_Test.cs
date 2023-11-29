using Broker.Services;
using ClassLibrary_SEP3;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq.Protected;
using Newtonsoft.Json;
using Task = ClassLibrary_SEP3.Task;
using TaskSys = System.Threading.Tasks.Task;


namespace Broker_Test
{

    public class SprintBacklogService_Test

    {
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly ISprintBacklogService _sprintBacklogService;

        public SprintBacklogService_Test()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://mockserver/")
            };
            _sprintBacklogService = new SprintBacklogService(_httpClient);
        }

        [Fact]
        public async void CreateSprintBacklogAsync_ReturnsSuccess()
        {
            // Arrange
            var sprintBacklog = new CreateSprintBackLogRequest
            {
                ProjectId = "1",
                Id = "1",
                Title = "Sample Sprint",
                Timestamp = DateTime.UtcNow,
                Tasks = new List<Task>()
            };
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(sprintBacklog))
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post && req.RequestUri.ToString()
                            .EndsWith($"api/Project/{sprintBacklog.ProjectId}/SprintBacklog")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _sprintBacklogService.CreateSprintBacklogAsync(sprintBacklog);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async void GetSprintBacklogsAsync_ReturnsListOfSprintBacklogs()
        {
            // Arrange
            var projectId = "1";
            var expectedSprintBacklogs = new List<SprintBacklog>
            {
                new SprintBacklog
                {
                    ProjectId = projectId,
                    SprintBacklogId = "2",
                    Title = "Alma",
                    Tasks = new List<Task>()
                },
            };
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedSprintBacklogs))
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString()
                            .EndsWith($"api/Project/{projectId}/SprintBacklog")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _sprintBacklogService.GetSprintBacklogsAsync(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var sprintBacklogs = Assert.IsAssignableFrom<List<SprintBacklog>>(okResult.Value);
            Assert.Equal(expectedSprintBacklogs.Count, sprintBacklogs.Count);

            for (int i = 0; i < expectedSprintBacklogs.Count; i++)
            {
                Assert.Equal(expectedSprintBacklogs[i].ProjectId, sprintBacklogs[i].ProjectId);
                Assert.Equal(expectedSprintBacklogs[i].SprintBacklogId, sprintBacklogs[i].SprintBacklogId);
            }
        }
        
        [Fact]
        public async void GetSprintBacklogByIdAsync_ReturnsSpecificSprintBacklog()
        {
            // Arrange
            var projectId = "1";
            var backlogId = "1";

            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = backlogId,
                Title = "Alma",
                CreatedAt = DateTime.UtcNow,
                Tasks = new List<Task>()
            };
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(expectedSprintBacklog))
            };

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().EndsWith($"api/Project/{projectId}/SprintBacklog/{backlogId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponse);

            // Act
            var result = await _sprintBacklogService.GetSprintBacklogByIdAsync(projectId, backlogId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var sprintBacklog = Assert.IsType<SprintBacklog>(okResult.Value);
            Assert.Equal(expectedSprintBacklog.ProjectId, sprintBacklog.ProjectId);
            Assert.Equal(expectedSprintBacklog.SprintBacklogId, sprintBacklog.SprintBacklogId);
        }




    }

}
