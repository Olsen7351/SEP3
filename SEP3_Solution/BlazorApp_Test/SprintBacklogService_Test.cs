using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest
{

    public class SprintBacklogServiceTests
    {
        [Fact]
        public async Task GetSprintBacklogByIdAsync_WhenSuccessful_ReturnsOkObjectResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:8002/")
            };
            var sprintBacklogService = new SprintBacklogService(httpClient);
            var projectId = "123";
            var SprintbacklogId = "456";
            var expectedSprintBacklog = new SprintBacklog
            {
                SprintBacklogId = SprintbacklogId,
                ProjectId = projectId,
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedSprintBacklog), Encoding.UTF8, "application/json")
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            // Act
            var result = await sprintBacklogService.GetSprintBacklogByIdAsync(projectId, SprintbacklogId);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<SprintBacklog>(okResult.Value);
            var actualSprintBacklog = (SprintBacklog)okResult.Value;
            Assert.Equal(expectedSprintBacklog.SprintBacklogId, actualSprintBacklog.SprintBacklogId);
            Assert.Equal(expectedSprintBacklog.ProjectId, actualSprintBacklog.ProjectId);
        }
        [Fact]
        public async Task GetSprintBacklogsAsync_WhenSuccessful_ReturnsOkObjectResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:8002/") // Replace with your base address
            };
            var sprintBacklogService = new SprintBacklogService(httpClient);
            var projectId = "123"; // Replace with a valid project ID
            var expectedSprintBacklogs = new List<SprintBacklog>
            {
                new SprintBacklog(),
                new SprintBacklog()
            };
            string json = JsonSerializer.Serialize(expectedSprintBacklogs);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            // Act
            var result = await sprintBacklogService.GetSprintBacklogsAsync(projectId);
            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsAssignableFrom<IEnumerable<SprintBacklog>>(okResult.Value);
            var sprintBacklogs = (IEnumerable<SprintBacklog>)okResult.Value;
            Assert.Equal(expectedSprintBacklogs.Count, sprintBacklogs.Count());
        }
        [Fact]
        public async Task CreateSprintBacklogAsync_WhenSuccessful_ReturnsOkObjectResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost:8002/")
            };
            var sprintBacklogService = new SprintBacklogService(httpClient);
            var expectedSprintBacklog = new SprintBacklog();
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedSprintBacklog), Encoding.UTF8, "application/json")
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            // Act
            var result = await sprintBacklogService.CreateSprintBacklogAsync(expectedSprintBacklog);
            // Assert
            Assert.IsType<CreatedResult>(result);
        }
    }
}
