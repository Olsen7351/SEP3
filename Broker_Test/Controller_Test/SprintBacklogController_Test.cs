using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = ClassLibrary_SEP3.Task;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;

namespace Broker_Test.Controller_Test
{
    public class SprintBacklogControllerTest
    {

        [Fact]
        public async void CreateSprintBacklog_ReturnsCreatedResult()
        {
            // Arrange
            var createSprintBacklogRequest = new CreateSprintBackLogRequest
            {
                // Assign necessary properties here
                projectId = "ProjectId",
                Title = "New Sprint",
                Timestamp = DateTime.Now,
                // Other properties if any
            };

            var mockService = new Mock<ISprintBacklogService>();
            mockService.Setup(service => service.CreateSprintBacklogAsync(createSprintBacklogRequest))
                .ReturnsAsync(new OkObjectResult(createSprintBacklogRequest)); // Assuming it returns the created object

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            var controller = new SprintBacklogController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.Post(createSprintBacklogRequest);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode); // Adjust as needed based on your implementation

            var returnedValue = Assert.IsType<CreateSprintBackLogRequest>(objectResult.Value);
            Assert.Equal(createSprintBacklogRequest, returnedValue); // Ensure the returned object is as expected

            // Verifying the service call
            mockService.Verify(service => service.CreateSprintBacklogAsync(createSprintBacklogRequest), Times.Once);
        }

        [Fact]
        public async void GetSpecificSprintBacklog_ReturnsValue()
        {
            // Arrange
            var projectId = "ProjectId";
            var sprintBacklogId = "5";
            var mockService = new Mock<ISprintBacklogService>();
            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = sprintBacklogId,
                Title = "Sample Sprint",
                CreatedAt = new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>()
            };

            mockService.Setup(service => service.GetSprintBacklogByIdAsync(projectId, sprintBacklogId))
                .ReturnsAsync(new OkObjectResult(expectedSprintBacklog));

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma"
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            var controller = new SprintBacklogController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.GetSpecificSprintBacklog(projectId, sprintBacklogId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<SprintBacklog>(objectResult.Value);
            Assert.Equal(expectedSprintBacklog,
                returnedValue); // Adjust the expected value according to your requirements

            // Verifying the service call
            mockService.Verify(service => service.GetSprintBacklogByIdAsync(projectId, sprintBacklogId), Times.Once);
        }

        [Fact]
        public async void AddTaskToSprintBacklogValid()
        {
            // Arrange
            var projectId = "1";
            var sprintBacklogId = "5";
            var mockService = new Mock<ISprintBacklogService>();
            var addTaskRequest = new AddSprintTaskRequest
            {
                ProjectId = projectId,
                SprintId = sprintBacklogId,
                Title = "Implement methods",
                Description = "Do it",
                Status = TaskStatus.ToDo,
                CreatedAt = DateTime.Now,
                EstimateTimeInMinutes = 120,
                ActualTimeUsedInMinutes = 0,
                Responsible = "Tom Riddle"
            };

            var mockTask = new Task
            {
                Id = "1",
                ProjectId = addTaskRequest.ProjectId,
                SprintId = addTaskRequest.SprintId,
                Title = addTaskRequest.Title,
                Description = addTaskRequest.Description,
                Status = addTaskRequest.Status,
                CreatedAt = addTaskRequest.CreatedAt,
                EstimateTimeInMinutes = addTaskRequest.EstimateTimeInMinutes,
                ActualTimeUsedInMinutes = addTaskRequest.ActualTimeUsedInMinutes,
                Responsible = addTaskRequest.Responsible
            };

            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = sprintBacklogId,
                Title = "Sample Sprint",
                CreatedAt = new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task> { mockTask }
            };

            mockService.Setup(service => service.AddTaskToSprintBacklogAsync(It.IsAny<AddSprintTaskRequest>()))
                .ReturnsAsync(new OkObjectResult(expectedSprintBacklog));

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma"
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            var controller = new SprintBacklogController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.AddTaskToSprintBacklog(addTaskRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var sprintBacklog = Assert.IsType<SprintBacklog>(okResult.Value);

            Assert.NotNull(result);
            Assert.Contains(sprintBacklog.Tasks,
                task => task.Title == mockTask.Title && task.Description == mockTask.Description);

            // Verifying the service call
            mockService.Verify(service => service.AddTaskToSprintBacklogAsync(It.IsAny<AddSprintTaskRequest>()),
                Times.Once);
        }

        [Fact]
        public async void GetAllTasksForSprintBacklog()
        {
            // Arrange
            var projectId = "1";
            var sprintBacklogId = "4";
            var mockService = new Mock<ISprintBacklogService>();
            var expectedTask = new List<Task>
            {
                new Task { Id = "1", ProjectId = "1", SprintId = "2", Title = "Task", Description = "Try and code" },
                new Task
                {
                    Id = "2", ProjectId = "1", SprintId = "2", Title = "Task 2",
                    Description = "I dont know what to put here"
                },
            };
            mockService.Setup(service => service.GetTasksFromSprintBacklogAsync(projectId, sprintBacklogId))
                .ReturnsAsync(new OkObjectResult(expectedTask));

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma"
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            var controller = new SprintBacklogController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.GetAllTasksForSprintBacklog(projectId, sprintBacklogId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<Task>>(okResult.Value);
            foreach (var task in expectedTask)
            {
                var actualTask = returnedTasks.FirstOrDefault(t => t.Id == task.Id);
                Assert.NotNull(actualTask); // Ensure the task is found
                Assert.Equal(task.ProjectId, actualTask.ProjectId);
                Assert.Equal(task.SprintId, actualTask.SprintId);
                Assert.Equal(task.Title, actualTask.Title);
            }

            // Verifying the service call
            mockService.Verify(service => service.GetTasksFromSprintBacklogAsync(projectId, sprintBacklogId),
                Times.Once);
        }

        [Fact]
        public async void Get_ReturnsSprintBacklogs()
        {
            // Arrange
            var projectId = "ProjectId";
            var mockService = new Mock<ISprintBacklogService>();
            var sprintBacklogs = new List<SprintBacklog>
            {
                new SprintBacklog { ProjectId = projectId, SprintBacklogId = "1", Title = "Sprint 1" },
                new SprintBacklog { ProjectId = projectId, SprintBacklogId = "2", Title = "Sprint 2" }
            };

            mockService.Setup(service => service.GetSprintBacklogsAsync(projectId))
                .ReturnsAsync(new OkObjectResult(sprintBacklogs));

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma" for rabbitMQ
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            var controller = new SprintBacklogController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var actionResult = await controller.GetAllSprintBacklogs(projectId);

            // Assert
            Assert.NotNull(actionResult);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(200, objectResult.StatusCode); // Check if the status code is OK (200)

            var model = Assert.IsAssignableFrom<IEnumerable<SprintBacklog>>(objectResult.Value);
            Assert.Equal(2, model.Count());

            // Verifying the service call
            mockService.Verify(service => service.GetSprintBacklogsAsync(projectId), Times.Once);
        }

        [Fact]
        public async void DeleteSprintBacklog_ReturnsOkResult()
        {
            // Arrange
            var projectId = "123"; // Example project ID
            var sprintBacklogId = "456"; // Example sprint backlog ID

            var mockService = new Mock<ISprintBacklogService>();
            mockService.Setup(service => service.DeleteSprintBacklogAsync(projectId, sprintBacklogId))
                .ReturnsAsync(new OkResult()); // Simulating a successful deletion

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Username"
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            var controller = new SprintBacklogController(mockService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.Delete(projectId, sprintBacklogId);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<OkResult>(result);

            // Verifying the service call
            mockService.Verify(service => service.DeleteSprintBacklogAsync(projectId, sprintBacklogId), Times.Once);
        }
    }
}
