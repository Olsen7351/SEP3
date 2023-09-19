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
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30)
        };

        // Act
        var actionResult = controller.CreateProject(request) as CreatedAtActionResult;
        var createdProject = actionResult?.Value as Project;

        // Assert
        Assert.NotNull(createdProject);
        Assert.Equal(request.Name, createdProject.Name);
        Assert.Equal(request.Description, createdProject.Description);
        Assert.Equal(request.StartDate, createdProject.StartDate);
        Assert.Equal(request.EndDate, createdProject.EndDate);
    }
}