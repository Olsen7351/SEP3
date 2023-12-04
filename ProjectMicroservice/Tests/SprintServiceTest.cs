using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using ProjectMicroservice.Controllers;
using ProjectMicroservice.Data;
using ProjectMicroservice.Services;
using Xunit;

namespace ProjectMicroservice.Tests;

public class SprintServiceTest
{
    private readonly Mock<ISprintService> _mockSprintService;
    private readonly SprintController _controller;
    

    public SprintServiceTest()
    {
        _mockSprintService = new Mock<ISprintService>();
        _controller = new SprintController(_mockSprintService.Object); 
    }
    [Fact]
    public void CreateSprint_WithValidModel_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var sprintRequest = new CreateSprintBackLogRequest { /* ... properties ... */ };
        var createdSprint = new SprintBacklog { /* ... properties ... */ };
        _mockSprintService.Setup(s => s.CreateSprintBacklog(It.IsAny<CreateSprintBackLogRequest>()))
            .Returns(createdSprint);

        // Act
        var result = _controller.CreateSprint(sprintRequest);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
        Assert.Equal(createdSprint, createdAtActionResult.Value);
        // Additional Assertions
    }
}