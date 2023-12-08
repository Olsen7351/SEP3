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
    
    
    //CreateNewEntryLogBook--------------------------------------------------------------------------
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

    
    
    [Fact]
    public async Task CreateLogBookEntry_EmptyProjectID()
    {
        // Arrange
        var logBookEntry = new AddEntryPointRequest()
        { 
            ProjectID = "",
            OwnerUsername = "James",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };

        _mockLogBookService.Setup(service => service.CreateNewEntryLogBook(It.IsAny<AddEntryPointRequest>()))
            .ReturnsAsync(It.IsAny<IActionResult>());

        // Act
        var exception = await Record.ExceptionAsync(() => _controller.CreateLogBookEntry(logBookEntry));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
        Assert.Equal("ProjectID cant be null or empty when creating a new entry", exception.Message);
    }


    [Fact]
    public async Task CreateLogBookEntry_EmptyUsername()
    {
        // Arrange
        var logBookEntry = new AddEntryPointRequest()
        { 
            ProjectID = "kksanfkajsnksnfa",
            OwnerUsername = "",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };

        _mockLogBookService.Setup(service => service.CreateNewEntryLogBook(It.IsAny<AddEntryPointRequest>()))
            .ReturnsAsync(It.IsAny<IActionResult>());

        // Act
        var exception = await Record.ExceptionAsync(() => _controller.CreateLogBookEntry(logBookEntry));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
        Assert.Equal("Username cant be null or empty when creating a new entry", exception.Message);
    }
    
   
    
    [Fact]
    public async Task CreateLogBookEntry_NullProjectID()
    {
        // Arrange
        var logBookEntry = new AddEntryPointRequest()
        { 
            ProjectID = null,
            OwnerUsername = "James",
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };

        _mockLogBookService.Setup(service => service.CreateNewEntryLogBook(It.IsAny<AddEntryPointRequest>()))
            .ReturnsAsync(It.IsAny<IActionResult>());

        // Act
        var exception = await Record.ExceptionAsync(() => _controller.CreateLogBookEntry(logBookEntry));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
        Assert.Equal("ProjectID cant be null or empty when creating a new entry", exception.Message);
    }
    
    
    
    [Fact]
    public async Task CreateLogBookEntry_NullUsername()
    {
        // Arrange
        var logBookEntry = new AddEntryPointRequest()
        { 
            ProjectID = "kksanfkajsnksnfa",
            OwnerUsername = null,
            Description = "Hey",
            CreatedTimeStamp = DateTime.Today
        };

        _mockLogBookService.Setup(service => service.CreateNewEntryLogBook(It.IsAny<AddEntryPointRequest>()))
            .ReturnsAsync(It.IsAny<IActionResult>());

        // Act
        var exception = await Record.ExceptionAsync(() => _controller.CreateLogBookEntry(logBookEntry));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<Exception>(exception);
        Assert.Equal("Username cant be null or empty when creating a new entry", exception.Message);
    }
    
    //GetEntriesForLogBook--------------------------------------------------------------------------------------------------------------------------
    
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
    
    //GetLogbookForProject ---------------------------------------------------------------------
    
    [Fact]
    public async Task GetLogbookForProject_EmptyProjectID()
    {
        // Arrange
        string projectId = ""; 

        // Act
        var actionResult = await _controller.GetLogbookForProject(projectId);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;

        // Assert
        Assert.NotNull(badRequestResult);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        Assert.Equal("ProjectID is required.", badRequestResult.Value);
    }
    
    
    [Fact]
    public async Task GetLogbookForProject_NullProjectID()
    {
        // Arrange
        string projectId = null;

        // Act
        var actionResult = await _controller.GetLogbookForProject(projectId);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;

        // Assert
        Assert.NotNull(badRequestResult);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        Assert.Equal("ProjectID is required.", badRequestResult.Value);
    }

    
    
    
    [Fact]
    public async Task CreateLogBookEntry_WithNullEntry()
    {
        // Act & Assert
        AddEntryPointRequest addEntryPointRequest = null;
        await Assert.ThrowsAsync<Exception>(() => _controller.CreateLogBookEntry(addEntryPointRequest));
    }
    
}