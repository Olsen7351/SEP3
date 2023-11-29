using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = ClassLibrary_SEP3.Task;

namespace Broker_Test.Controller_Test
{
    public class SprintBacklogControllerTest
    {
        [Fact]
        public async void Get_ReturnsSprintBacklogs2()
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

            var controller = new SprintBacklogController(mockService.Object);
            var actionResult = await controller.GetAllSprintBacklogs(projectId);
            
            Assert.NotNull(actionResult);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsAssignableFrom<IEnumerable<SprintBacklog>>(objectResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async void AddTaskToSprintBacklog_ReturnsValue()
        {
            var mockService = new Mock<ISprintBacklogService>();
            var controller = new SprintBacklogController(mockService.Object);
            var projectId = "1";
            var sprintBacklogId = "2";
            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = sprintBacklogId,
                Title = "Sample Sprint",
                CreatedAt= new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>()
            };
            var sprintId = "2";
            var task = new Task
            {
                SprintId = "2",
                Title = "Brush Alma"
            };
            mockService.Setup(service => service.AddTaskToSprintBacklogAsync("1", "2", task))
                .ReturnsAsync(new OkObjectResult(expectedSprintBacklog));
            var result = await controller.AddTaskToSprintBacklog("1", "2", task);
            Assert.NotNull(result);

        }
        
        [Fact]
        public async void Post_CreatesSprintBacklog2()
        {
            // Arrange
            var mockService = new Mock<ISprintBacklogService>();
            var controller = new SprintBacklogController(mockService.Object);

            var sprintBacklogData = new SprintBacklog
            {
                ProjectId = "sampleProjectId",
                SprintBacklogId = "1",
                Title = "Sample Sprint",
                CreatedAt = new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>()
            };

            mockService.Setup(service => service.CreateSprintBacklogAsync(It.IsAny<SprintBacklog>()))
                .ReturnsAsync(new CreatedAtActionResult(nameof(SprintBacklogController.GetSpecificSprintBacklog), "SprintBacklog", new { id = sprintBacklogData.SprintBacklogId }, sprintBacklogData));

            // Act
            var actionResult = await controller.Post(sprintBacklogData);

            // Assert
            mockService.Verify(service => service.CreateSprintBacklogAsync(It.IsAny<SprintBacklog>()), Times.Once);
            Assert.NotNull(actionResult);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
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
                CreatedAt= new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>()
            };

            // Mock the behavior of GetSprintBacklogByIdAsync to return a specific backlog
            mockService.Setup(service => service.GetSprintBacklogByIdAsync(projectId, sprintBacklogId))
                .ReturnsAsync(new OkObjectResult(expectedSprintBacklog));

            var controller = new SprintBacklogController(mockService.Object);

            // Act
            var result = await controller.GetSpecificSprintBacklog(projectId, sprintBacklogId);

            // Assert
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<SprintBacklog>(objectResult.Value);
            Assert.Equal(expectedSprintBacklog, returnedValue); // Adjust the expected value according to your requirements
        }

        [Fact]
        public void AddTaskToSprintBacklogValid()
        {
            var projectId = "1";
            var sprintBacklogId = "5";
            var mockService = new Mock<ISprintBacklogService>();
            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = sprintBacklogId,
                Title = "Sample Sprint",
                CreatedAt= new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>()
            }; 
            var task = new Task
            {
                Id 
            }
        }
    }
}
