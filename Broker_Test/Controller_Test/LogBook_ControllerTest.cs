using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = System.Threading.Tasks.Task;

namespace Broker_Test.Controller_Test;

public class LogBook_ControllerTest
{
    
    private readonly Mock<ILogBookService> _mockLogBookService;
    private readonly LogBookController _controller;

    public LogBook_ControllerTest()
    {
        _mockLogBookService = new Mock<ILogBookService>();
        _controller = new LogBookController(_mockLogBookService.Object);
    }
    
    
    //LogBook Entry
    [Fact]
    public async Task CreateLogBookEntry_WithValidEntry()
    {
        // Arrange
        var logBookEntry = new LogBookEntryPoints
        {
            LogBookID = 1,
            OwnerUsername = "James",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };
        var serviceResult = new OkResult(); 
        _mockLogBookService.Setup(service => service.CreateNewEntryLogBook(It.IsAny<LogBookEntryPoints>()))
            .ReturnsAsync(serviceResult);

        // Act
        var result = await _controller.CreateLogBookEntry(logBookEntry);

        // Assert
        Assert.IsType<OkResult>(result);
    }
    
    
    //Logbook
    [Fact]
    public async Task GetLogbook_WithValidProjectId()
    {
        // Arrange
        string projectID = "IAmAProjectID";
        var mockEntries = new List<LogBookEntryPoints>(); 
        var mockResult = new OkObjectResult(mockEntries); 

        _mockLogBookService.Setup(service => service.GetEntriesForLogBook(projectID))
            .ReturnsAsync(mockResult); 

        // Act
        var result = await _controller.GetLogbook(projectID); 

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode); 
    }

    
    
    
    [Fact]
    public async Task CreateLogBookEntry_WithNullEntry()
    {
        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.CreateLogBookEntry(null));
    }
    
}