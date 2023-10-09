using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Controllers;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;
using ProjectMicroservice.Services;
using Xunit;

namespace ProjectMicroservice.Tests;

public class ProjectControllerTests
{
    [Fact]
    public void CreateProject_ValidRequest_ReturnsExpectedProject()
    {
        // Arrange
        var service = new ProjectService();
        var controller = new ProjectController(service);
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 1)
        };

        // Act
        var actionResult = controller.CreateProject(request) as CreatedAtActionResult;

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
        var service = new ProjectService();
        var controller = new ProjectController(service);
        var request = new CreateProjectRequest
        {
            Description = "Test Description",
            StartDate = new DateTime(2023, 1, 1),
            EndDate = new DateTime(2023, 2, 1)
        };

        // Act
        var actionResult = controller.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }
    
    [Fact]
    public void CreateProject_MissingStartDate_ReturnsBadRequest()
    {
        // Arrange
        var service = new ProjectService();
        var controller = new ProjectController(service);
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            EndDate = new DateTime(2023, 2, 1)
        };

        // Act
        var actionResult = controller.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }

    [Fact]
    public void CreateProject_MissingEndDate_ReturnsBadRequest()
    {
        // Arrange
        var service = new ProjectService();
        var controller = new ProjectController(service);
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = new DateTime(2023, 1, 1)
        };

        // Act
        var actionResult = controller.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }

    [Fact]
    public void CreateProject_MissingBothDates_ReturnsBadRequest()
    {
        // Arrange
        var service = new ProjectService();
        var controller = new ProjectController(service);
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description"
        };

        // Act
        var actionResult = controller.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode); // HTTP 400 Bad Request
    }

    
    [Fact]
    public void CreateProject_StartDateAfterEndDate_ReturnsBadRequest()
    {
        // Arrange
        var service = new ProjectService();
        var controller = new ProjectController(service);
        var request = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "Test Description",
            StartDate = new DateTime(2023, 2, 1),
            EndDate = new DateTime(2023, 1, 1)
        };

        // Act
        var actionResult = controller.CreateProject(request) as BadRequestResult;

        // Assert
        Assert.NotNull(actionResult);
        Assert.Equal(400, actionResult.StatusCode);
    }
}