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
using ClassLibrary_SEP3.DataTransferObjects;
using Xunit;
using Task = ClassLibrary_SEP3.Task;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;


namespace BlazorAppTest
{

    public class SprintBacklogServiceTests
    {
        [Fact]
        public async void GetSprintBacklogByIdAsync_WhenSuccessful_ReturnsOkObjectResult()
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
        public async void GetSprintBacklogsAsync_WhenSuccessful_ReturnsOkObjectResult()
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
        public async void CreateSprintBacklogAsync_WhenSuccessful_ReturnsOkObjectResult()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
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
            Assert.IsType<OkObjectResult>(result);
            //Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async void AddTaskToSprintBacklog()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            var sprintBacklogService = new SprintBacklogService(httpClient);
            var projectId = "1";
            var sprintBacklogId = "2";
            var addTaskRequest = new AddSprintTaskRequest()
            {
                ProjectId = "1",
                SprintId = "5",
                Title = "Implement methods",
                Description = "Do it",
                Status = TaskStatus.ToDo, 
                CreatedAt = DateTime.Now,
                EstimateTimeInMinutes = 120,
                ActualTimeUsedInMinutes = 0,
                Responsible = "Tom Riddle"
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(addTaskRequest), Encoding.UTF8, "application/json")
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var result =
                await sprintBacklogService.AddTaskToSprintBacklogAsync(projectId, sprintBacklogId, addTaskRequest);
            
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var returnedTask = JsonSerializer.Deserialize<AddSprintTaskRequest>(okResult.Value.ToString());
            Assert.NotNull(result);
            Assert.Equal(addTaskRequest.Title, returnedTask.Title);
            Assert.Equal(addTaskRequest.Description, returnedTask.Description);
        }

        [Fact]
        public async void AddTaskToNullSprintBacklog()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            var sprintBacklogService = new SprintBacklogService(httpClient);
            var projectId = "1";
            string sprintBacklogId = null; 
            
            var addTaskRequest = new AddSprintTaskRequest()
            {
                ProjectId = "1",
                SprintId = "5",
                Title = "Implement methods",
                Description = "Do it",
                Status = TaskStatus.ToDo, 
                CreatedAt = DateTime.Now,
                EstimateTimeInMinutes = 120,
                ActualTimeUsedInMinutes = 0,
                Responsible = "Tom Riddle"
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Sprint backlog ID cannot be null."));
            await Assert.ThrowsAsync<HttpRequestException>(() => sprintBacklogService.AddTaskToSprintBacklogAsync(projectId, sprintBacklogId, addTaskRequest));
        }
        [Fact]
        public async void GetAllTasksForSprintBackog()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            var sprintBacklogService = new SprintBacklogService(httpClient);
            var projectId = "1";
            var sprintBacklogId = "2";
            
            var expectedTask = new List<Task>
            {
                
                new Task { Id= "1", ProjectId = "1", SprintId = "2", Title = "Task", Description = "Try and code" },
                new Task { Id = "2", ProjectId = "1", SprintId = "2", Title = "Task 2", Description = "I dont know what to put here" },
            };
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedTask), Encoding.UTF8, "application/json")
            };
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
            var result = await sprintBacklogService.GetTasksFromSprintBacklogAsync(projectId, sprintBacklogId);
            
            Assert.IsType<OkObjectResult>(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsAssignableFrom<IEnumerable<Task>>(okResult.Value);
        }

        [Fact]
        public async void GetTasksFromSprintBackLogSWhenSprintBacklogIdIsNull()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://localhost/")
            };
            var sprintBacklogService = new SprintBacklogService(httpClient);
            var projectId = "1";
            string sprintBacklogId = null; 
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Sprint backlog not found."));
           
            await Assert.ThrowsAsync<HttpRequestException>(() => sprintBacklogService.GetTasksFromSprintBacklogAsync(projectId, sprintBacklogId));
        }
    }
}
