using Xunit;
using Microsoft.AspNetCore.Mvc;
using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;

namespace Broker_Test;

public class UnitTest1
{
    [Fact]
    public void GetProjekt_ReturnsOkResult()
    {
        // Arrange
        var mockProjektService = MockTestSetupService.CreateMockProjektService();
        var controller = new ProjektController(mockProjektService.Object);

        // Act
        var result = controller.GetProjekt(1);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public void CreateProjekt_ReturnsOkResult()
    {
        // Arrange
        var mockProjektService = MockTestSetupService.CreateMockProjektService();
        var controller = new ProjektController(mockProjektService.Object);

        // Act
        var result = controller.CreateProjekt(new Project
        {
            Name = "New_Name",
            Description = "En Beskrivelse",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(30),
        });

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}