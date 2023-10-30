using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using ProjectMicroservice.Controllers;
using ProjectMicroservice.Data;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;
using ProjectMicroservice.Services;
using System;
using Xunit;

namespace ProjectMicroservice.Tests
{
    public class BacklogControllerTests
    {
        private readonly Mock<IMongoCollection<BacklogDatabase>> _mockBacklogCollection;
        private readonly Mock<IMongoCollection<ProjectDatabase>> _mockProjectCollection;
        private readonly Mock<MongoDbContext> _mockDbContext;
        private readonly BacklogService _backlogService;
        private readonly ProjectService _projectService;
        private readonly BacklogController _backlogController;
        private readonly TaskService _taskService;
        

        public BacklogControllerTests()
        {
            // Mock the MongoDB collections
            _mockBacklogCollection = new Mock<IMongoCollection<BacklogDatabase>>();
            _mockProjectCollection = new Mock<IMongoCollection<ProjectDatabase>>();

            // Mock the MongoDB context
            _mockDbContext = new Mock<MongoDbContext>("mongodb://localhost:27017", "test_db");
            _mockDbContext
                .Setup(db => db.Database.GetCollection<BacklogDatabase>(
                    It.IsAny<string>(),
                    It.IsAny<MongoCollectionSettings>()
                ))
                .Returns(_mockBacklogCollection.Object);

            _mockDbContext
                .Setup(db => db.Database.GetCollection<ProjectDatabase>(
                    It.IsAny<string>(),
                    It.IsAny<MongoCollectionSettings>()
                ))
                .Returns(_mockProjectCollection.Object);

            // Initialize the services and controller
            _backlogService = new BacklogService(_mockDbContext.Object);
            _projectService = new ProjectService(_mockDbContext.Object);
            _taskService = new TaskService(_mockDbContext.Object);
            
            _backlogController = new BacklogController(_backlogService, _projectService, _taskService);
        }

        [Fact]
        public void CreateBacklog_ValidRequest_ReturnsExpectedBacklog()
        {
            // Arrange
            var projectRequest = new CreateProjectRequest
            {
                Name = "Test Project",
                Description = "Test Description",
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 12, 31)
            };

            var project = _projectService.CreateProject(projectRequest);  // Create a dummy project
            var projectId = project.Id;

            var request = new CreateBacklogRequest
            {
                Description = "Test Backlog Description"
            };

            // Act
            var actionResult = _backlogController.CreateBacklog(projectId.ToString(), request) as CreatedAtActionResult;

            // Assert
            Assert.NotNull(actionResult);
            // 201 or 200
            Assert.True(actionResult.StatusCode == 201 || actionResult.StatusCode == 200);

            var createdBacklog = actionResult?.Value as BacklogDatabase;
            Assert.NotNull(createdBacklog);

            Assert.Equal(request.Description, createdBacklog?.Description);
        }

        [Fact]
        public void CreateBacklog_ProjectNotFound_ReturnsNotFound()
        {  
            // Arrange
            // Non-existent project ID
            var projectId = new ObjectId("5f9a4f8f9d3f2f0a8c7d1b1a");

            var request = new CreateBacklogRequest
            {
                Description = "Test Backlog Description"
            };

            // Act
            var actionResult = _backlogController.CreateBacklog(projectId.ToString(), request) as NotFoundResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(404, actionResult.StatusCode);
        }

        [Fact]
        public void CreateBacklog_ProjectAlreadyHasBacklog_ReturnsConflict()
        {
            // Arrange
            var projectRequest = new CreateProjectRequest
            {
                Name = "Test Project",
                Description = "Test Description",
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 12, 31)
            };
            var project = _projectService.CreateProject(projectRequest);  // Create a dummy project
            var projectId = project.Id;

            var request = new CreateBacklogRequest
            {
                Description = "Test Backlog Description"
            };

            _backlogService.CreateBacklog(projectId, request);  // Create a dummy backlog

            // Act
            var actionResult = _backlogController.CreateBacklog(projectId.ToString(), request) as ConflictResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(409, actionResult.StatusCode);
        }
    }
}
