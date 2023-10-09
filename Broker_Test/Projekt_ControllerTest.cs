using Broker.Controllers;
using Broker.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectMicroservice.Models;

namespace Broker_Test
{
    public class ProjektControllerTests
    {
        [Fact]
        public void GetProjekt_ReturnsOk_WhenIdIsValid()
        {
            // Arrange
            var mockProjektService = new Mock<IProjektService>();
            var controller = new ProjektController(mockProjektService.Object);
            int validId = 1; // Valid Id

            // Mock the ProjektService to return a sample Project
            mockProjektService.Setup(service => service.GetProjekt(validId))
                .ReturnsAsync(new OkObjectResult(new Project()));

            // Act
            var result = controller.GetProjekt(validId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetProjekt_ReturnsBadRequest_WhenIdIsNegative()
        {
            // Arrange
            var mockProjektService = new Mock<IProjektService>();
            var controller = new ProjektController(mockProjektService.Object);
            int negativeId = -1; // Negative Id

            // Act
            var result = controller.GetProjekt(negativeId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CreateProjekt_ReturnsOk_WhenProjectIsNotNull()
        {
            // Arrange
            var mockProjektService = new Mock<IProjektService>();
            var controller = new ProjektController(mockProjektService.Object);
            var validProject = new Project(); // Valid Project

            // Mock the ProjektService to return IActionResult (e.g., OkResult)
            mockProjektService.Setup(service => service.CreateProjekt(validProject))
                .ReturnsAsync(new OkResult());

            // Act
            var result = controller.CreateProjekt(validProject);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void CreateProjekt_ReturnsBadRequest_WhenProjectIsNull()
        {
            // Arrange
            var mockProjektService = new Mock<IProjektService>();
            var controller = new ProjektController(mockProjektService.Object);
            Project nullProject = null; // Project is null

            // Act
            var result = controller.CreateProjekt(nullProject);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
