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
    
    
    //CreateLogBookEntry--------------------------------------------------------------------------
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
    public async Task CreateLogBookEntry_WithNullEntry()
    {
        // Act & Assert
        AddEntryPointRequest addEntryPointRequest = null;
        await Assert.ThrowsAsync<Exception>(() => _controller.CreateLogBookEntry(addEntryPointRequest));
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
    
    //GetLogbookForProject--------------------------------------------------------------------------------------------------------------------------
    
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
    
    
    //UpdateEntry----------------------------
    [Fact]
    public async Task UpdateEntry_WithValidInformation()
    {
        // Arrange
        var updateRequest = new UpdateEntryRequest()
        {
            ProjectID = "sdasdasda",
            EntryID = "857sadk2381",
            Description = "I AM A TEST FOR TESTING"
        };

        _mockLogBookService.Setup(service => service.UpdateEntry(It.IsAny<UpdateEntryRequest>()))
            .ReturnsAsync(new OkObjectResult("Entry updated successfully."));

        // Act
        var result = await _controller.UpdateEntry(updateRequest);

        // Assert
        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Entry updated successfully.", okObjectResult.Value);
    }

    
    
    [Fact]
    public async Task UpdateEntryObjectNull()
    {
        // Arrange
        UpdateEntryRequest updateRequest = null;

        // Act
        var result = await _controller.UpdateEntry(updateRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Request payload cannot be null", badRequestResult.Value);
    }
    
    
    [Fact]
    public async Task UpdateEntry_WithEmptyProjectID()
    {
        // Arrange
        var updateRequest = new UpdateEntryRequest()
        {
            ProjectID = "",
            EntryID = "857sadk2381",
            Description = "I AM A TEST FOR TESTING"
        };
        
        // Act
        var result = await _controller.UpdateEntry(updateRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("EntryID and ProjectID must not be null or empty.", badRequestResult.Value);
    }
    
    
    [Fact]
    public async Task UpdateEntry_WithNullProjectID()
    {
        // Arrange
        var updateRequest = new UpdateEntryRequest()
        {
            ProjectID = null,
            EntryID = "857sadk2381",
            Description = "I AM A TEST FOR TESTING"
        };
        
        // Act
        var result = await _controller.UpdateEntry(updateRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("EntryID and ProjectID must not be null or empty.", badRequestResult.Value);
    }
    
    
    
    //GetSpecificEntry-----------------------

    [Fact]
    public async Task GetSpecificEntry_WithVaildInformation()
    {
        // Arrange
        string projectId = "validProjectId";
        string entryId = "validEntryId";
        var expectedEntry = new LogBookEntryPoints();
        _mockLogBookService.Setup(service => service.GetSpecificEntry(projectId, entryId))
            .ReturnsAsync(expectedEntry);

        // Act
        var result = await _controller.GetSpecificEntry(projectId, entryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(expectedEntry, okResult.Value);
    }

    [Fact]
    public async Task GetSpecificEntry_EmptyProjectID()
    {
        string projectId = "";
        string entryId = "sakjfnajskfnasf";
        
        // Act
        var result = await _controller.GetSpecificEntry(projectId, entryId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("EntryID and ProjectID must not be null or empty.", badRequestResult.Value);
    }
    
    
    
    [Fact]
    public async Task GetSpecificEntry_NullProjectID()
    {
        string projectId = null;
        string entryId = "sakjfnajskfnasf";
        
        // Act
        var result = await _controller.GetSpecificEntry(projectId, entryId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("EntryID and ProjectID must not be null or empty.", badRequestResult.Value);
    }
    
    
    [Fact]
    public async Task GetSpecificEntry_EmptyEntryID()
    {
        string projectId = "safasfsafa";
        string entryId = "";
        
        // Act
        var result = await _controller.GetSpecificEntry(projectId, entryId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("EntryID and ProjectID must not be null or empty.", badRequestResult.Value);
    }
    
    
    
    [Fact]
    public async Task GetSpecificEntry_NullEntryID()
    {
        string projectId = "safasfsafa";
        string entryId = null;
        
        // Act
        var result = await _controller.GetSpecificEntry(projectId, entryId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("EntryID and ProjectID must not be null or empty.", badRequestResult.Value);
    }
}