using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClassLibrary_SEP3;
using ProjectMicroservice.Controllers;
using ProjectMicroservice.Services;
using Task = System.Threading.Tasks.Task;

public class LogBookControllerTests
{
    
    //Update Logbook Entry-----------------------------------------------------------------------------
    [Fact]
    public async Task UpdateLogBookEntryWhenPayloadIsNull()
    {
        // Arrange
        var mockService = new Mock<ILogBookService>();
        var controller = new LogBookController(mockService.Object);

        // Act
        var result = await controller.UpdateLogBookEntry(null);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
    
    [Fact]
    public async Task UpdateLogBookWhereProjectIDDoesntExist()
    {
        // Arrange
        var mockService = new Mock<ILogBookService>();
        mockService.Setup(service => service.UpdateLogBookEntry(It.IsAny<UpdateEntryRequest>()))
            .ReturnsAsync(false);
        var controller = new LogBookController(mockService.Object);
        var updateEntryRequest = new UpdateEntryRequest { ProjectID = "123", EntryID = "456", Description = "New Description" };

        // Act
        var result = await controller.UpdateLogBookEntry(updateEntryRequest);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    //Stopped testing controllers
}
