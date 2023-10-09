using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Controllers;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;
using ProjectMicroservice.Services;

namespace ProjectMicroseriveTest
{
    public class BacklogControllerTests
    {
        [Fact]
        public void CreateBacklog_ValidRequest_ReturnsExpectedBacklog()
        {
            // Arrange
            var backlogService = new BacklogService();
            var projectService = new ProjectService();
            var controller = new BacklogController(backlogService, projectService);

            var project = projectService.CreateProject(new Project());  // Create a dummy project
            var projectId = project.Id;

            var request = new CreateBacklogRequest
            {
                Description = "Test Backlog Description"
            };

            // Act
            var actionResult = controller.CreateBacklog(projectId, request) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(actionResult);
            // 201 or 200
            Assert.True(actionResult.StatusCode == 201 || actionResult.StatusCode == 200);

            var createdBacklog = actionResult?.Value as Backlog;
            Assert.NotNull(createdBacklog);

            Assert.Equal(request.Description, createdBacklog?.Description);
        }

        [Fact]
        public void CreateBacklog_ProjectNotFound_ReturnsNotFound()
        {
            // Arrange
            var backlogService = new BacklogService();
            var projectService = new ProjectService();
            var controller = new BacklogController(backlogService, projectService);

            var projectId = 9999;  // Simulate a non-existing project ID

            var request = new CreateBacklogRequest
            {
                Description = "Test Backlog Description"
            };

            // Act
            var actionResult = controller.CreateBacklog(projectId, request) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public void CreateBacklog_ProjectAlreadyHasBacklog_ReturnsConflict()
        {
            // Arrange
            var backlogService = new BacklogService();
            var projectService = new ProjectService();
            var controller = new BacklogController(backlogService, projectService);

            var project = projectService.CreateProject(new Project());  // Create a dummy project
            var projectId = project.Id;

            var request = new CreateBacklogRequest
            {
                Description = "Test Backlog Description"
            };

            backlogService.CreateBacklog(projectId, new Backlog());  // Create a dummy backlog

            // Act
            var actionResult = controller.CreateBacklog(projectId, request) as ConflictResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(409, actionResult.StatusCode);
        }
    }
}
