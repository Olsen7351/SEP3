using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest
{
    
    public class SprintBacklogService_Test
    {
        private Mock<ISprintBacklogService> _mockService;
        private SprintBacklogService _service;

        public SprintBacklogService_Test()
        {
            _mockService = new Mock<ISprintBacklogService>();
            _service = new SprintBacklogService();
        }

        [Fact]
        public async Task CreateSprintBacklogAsync_ValidInput_ReturnsOkResult()
        {
            // Arrange
            var sprintBacklog = new SprintBacklog(); // Create a sample SprintBacklog object
            _mockService.Setup(service => service.CreateSprintBacklogAsync(sprintBacklog))
                        .ReturnsAsync(new OkResult()); // Simulating the API call behavior

            // Act
            var result = await _mockService.Object.CreateSprintBacklogAsync(sprintBacklog) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task GetSprintBacklogsAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var projectId = "sampleProjectId";
            _mockService.Setup(service => service.GetSprintBacklogsAsync(projectId))
                .ReturnsAsync(new OkObjectResult("Sample data")); // Simulating the API call behavior

            // Act
            var result = await _mockService.Object.GetSprintBacklogsAsync(projectId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Sample data", result.Value); // Ensure the returned data is as expected
        }

        [Fact]
        public async Task GetSprintBacklogByIdAsync_ReturnsOkObjectResult()
        {
            // Arrange
            var projectId = "sampleProjectId";
            var id = "sampleId";
            _mockService.Setup(service => service.GetSprintBacklogByIdAsync(projectId, id))
                .ReturnsAsync(new OkObjectResult("Sample sprint backlog")); // Simulating the API call behavior

            // Act
            var result = await _mockService.Object.GetSprintBacklogByIdAsync(projectId, id) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Sample sprint backlog", result.Value); // Ensure the returned data is as expected
        }
    }
}
