using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Services;
using ClassLibrary_SEP3;
using ProjectMicroservice.Controllers;


public class LogBookControllerTests
{
    
    
    private readonly Mock<ILogBookService> _mockLogBookService;
    private readonly Mock<IProjectService> _mockProjectService;
    private readonly LogBookController _logBookController;

    public LogBookControllerTests()
    {
        // Mock the services
        _mockLogBookService = new Mock<ILogBookService>();
        _mockProjectService = new Mock<IProjectService>();

        // Setup the controller
        _logBookController = new LogBookController(_mockLogBookService.Object);
    }
    
    
    /* ON HOLD
    [Fact]
    public async Task GetLogbookForProject_AfterCreatingEntries_ReturnsLogBookWithEntries()
    {
        // Arrange
        var projectId = "validProjectId";
        var logBook = new LogBook
        {
            ProjectID = projectId,
            LogBookEntryPoints = new List<LogBookEntryPoints>
            {
                new LogBookEntryPoints
                {
                    LogBookID = 1,
                    OwnerUsername = "testUser1",
                    Description = "Test Description 1",
                    CreatedTimeStamp = DateTime.UtcNow
                },
                new LogBookEntryPoints
                {
                    LogBookID = 2,
                    OwnerUsername = "testUser2",
                    Description = "Test Description 2",
                    CreatedTimeStamp = DateTime.UtcNow.AddDays(-1)
                }
            }
        };

        _mockLogBookService.Setup(service => service.GetLogbookForProject(It.IsAny<string>()))
            .ReturnsAsync(logBook); 

        // Act
        var result = await _logBookController.GetLogbookForProject(projectId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedLogBook = Assert.IsType<LogBook>(okResult.Value);
        Assert.Equal(200, okResult.StatusCode);
        Assert.NotNull(returnedLogBook.LogBookEntryPoints);
        Assert.Equal(2, returnedLogBook.LogBookEntryPoints.Count);
        Assert.Equal(projectId, returnedLogBook.ProjectID);
        Assert.All(returnedLogBook.LogBookEntryPoints, entry =>
        {
            Assert.NotNull(entry.OwnerUsername);
            Assert.NotNull(entry.Description);
        });
    }



    
    

    [Fact]
    public async Task CreateLogBookEntry_WithValidEntry_ReturnsOkStatusCode()
    {
        // Arrange
        var mockService = new Mock<ILogBookService>();
        var logBookEntry = new LogBookEntryPoints(); // Populate with valid data
        mockService.Setup(service => service.CreateNewEntry(logBookEntry))
            .Returns(new LogBookEntry()); // Replace with your actual return type

        var controller = new LogBookController(mockService.Object);

        // Act
        var result = await controller.CreateLogBookEntry(logBookEntry);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    */
}