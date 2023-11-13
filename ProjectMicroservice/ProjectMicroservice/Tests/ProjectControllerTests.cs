using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using ProjectMicroservice.Controllers;
using ProjectMicroservice.Data;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Services;
using Xunit;

namespace ProjectMicroservice.Tests;

public class ProjectControllerTests
{
    private readonly Mock<IMongoCollection<ProjectDatabase>> _mockCollection;
    private readonly Mock<MongoDbContext> _mockDbContext;
    private readonly ProjectService _projectService;
    private readonly ProjectController _brokerProjectController;

    public ProjectControllerTests()
    {
        // Mock the MongoDB collection
        _mockCollection = new Mock<IMongoCollection<ProjectDatabase>>();

        // Mock the MongoDB context
        _mockDbContext = new Mock<MongoDbContext>("mongodb://localhost:27017", "test_db");
        _mockDbContext
        .Setup(db => db.Database.GetCollection<ProjectDatabase>(
        It.IsAny<string>(),
        It.IsAny<MongoCollectionSettings>()
        ))
        .Returns(_mockCollection.Object);

        // Initialize the service and controller
        _projectService = new ProjectService(_mockDbContext.Object);
        _brokerProjectController = new ProjectController(_projectService);
    }

    [Fact]
    public void CreateProject_ValidRequest_ReturnsExpectedProject()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 1)
        };

        // Act
        var actionResult = _brokerProjectController.CreateProject(request) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(201, actionResult.StatusCode); // Checking HTTP 201 Created status

        var createdProject = actionResult?.Value as Project;
        Assert.NotNull(createdProject);
    
        Assert.Equal(request.Name, createdProject?.Name);
        Assert.Equal(request.Description, createdProject?.Description);
        Assert.Equal(request.StartDate, createdProject?.StartDate);
        Assert.Equal(request.EndDate, createdProject?.EndDate);
    }
    
    [Fact]
    public void CreateProject_MissingName_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Description = "Test Description",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 1)
        };

        // Act
        var actionResult = _brokerProjectController.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }
    
    [Fact]
    public void CreateProject_MissingStartDate_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            EndDate = new DateTime(2023, 2, 1)
        };

        // Act
        var actionResult = _brokerProjectController.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }

    [Fact]
    public void CreateProject_MissingEndDate_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = new DateTime(2023, 1, 1)
        };

        // Act
        var actionResult = _brokerProjectController.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }

    [Fact]
    public void CreateProject_MissingBothDates_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description"
        };

        // Act
        var actionResult = _brokerProjectController.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }

    
    [Fact]
    public void CreateProject_StartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = new DateTime(2023, 2, 1),
            EndDate = new DateTime(2023, 1, 1)
        };

        // Act
        var actionResult = _brokerProjectController.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode);
    }
}

internal class ProjectDatabase
{
}