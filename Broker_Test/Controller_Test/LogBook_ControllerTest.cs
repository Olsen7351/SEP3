using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
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
        var logBookEntry = new AddEntryPointRequest()
        { 
            ProjectID = "asfjashfajf",
            OwnerUsername = "James",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };
        var serviceResult = new OkResult(); 
        _mockLogBookService.Setup(service => service.CreateNewEntryLogBook(It.IsAny<AddEntryPointRequest>()))
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
        var mockLogBook = new LogBook { LogBookEntryPoints = mockEntries }; // Create a mock LogBook object

        _mockLogBookService.Setup(service => service.GetEntriesForLogBook(projectID))
            .ReturnsAsync(mockLogBook); // Return the mock LogBook object

        // Act
        var result = await _controller.GetLogbookForProject(projectID); 

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); 
        Assert.Equal(mockLogBook, okResult.Value); 
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode); 
    }


    
    
    
    [Fact]
    public async Task CreateLogBookEntry_WithNullEntry()
    {
        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.CreateLogBookEntry(null));
    }
    
}