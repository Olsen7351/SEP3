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
using Task = System.Threading.Tasks.Task;

namespace Broker_Test.Controller_Test
{
    public class SprintBacklogControllerTest
    {
        [Fact]
        public async Task Get_ReturnsSprintBacklogs2()
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
        public async Task Post_CreatesSprintBacklog2()
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
        public async Task GetSpecificSprintBacklog_ReturnsValue()
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
                 public async Task Get_ReturnsSprintBacklogs()
                 {
                     // Arrange
                     var projectId = "ProjectId";
                     var mockService = new Mock<ISprintBacklogService>();
                     var sprintBacklogs = new List<SprintBacklog>
                     {
                         new SprintBacklog { ProjectId = projectId, SprintBacklogId = "1", Title = "Sprint 1" },
                         new SprintBacklog { ProjectId = projectId, SprintBacklogId = "2", Title = "Sprint 2" }
                     };
                     mockService.Setup(service => service.GetSprintBacklogsAsync("ProjectId"))
                         .ReturnsAsync(new OkObjectResult(sprintBacklogs));
         
                     var controller = new SprintBacklogController(mockService.Object);
         
                     // Act
                     var result = await controller.GetAllSprintBacklogs(projectId) as Task<IEnumerable<SprintBacklog>>;
         
                     // Assert
                     Assert.NotNull(result);
                     var objectResult = Assert.IsType<OkObjectResult>(result.Result);
                     var model = Assert.IsAssignableFrom<IEnumerable<SprintBacklog>>(objectResult.Value);
                     Assert.Equal(2, model.Count());
                 }
        [Fact]
        public async Task Post_CreatesSprintBacklog()
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

            
            // Act
            var result = await controller.Post(sprintBacklogData) as Task<IActionResult>;

            // Assert
            mockService.Verify(service => service.CreateSprintBacklogAsync(sprintBacklogData), Times.Once);
            Assert.IsType<CreatedAtActionResult>(result); 
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }
    }
}
