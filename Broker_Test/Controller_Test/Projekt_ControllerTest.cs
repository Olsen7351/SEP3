using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectMicroservice.DataTransferObjects;

namespace Broker_Test
{
    public class ProjektControllerTests
    {
        
        [Fact]
        
        public async void GetProjekt_ReturnsOk_WhenIdIsValid()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new BrokerProjectController(mockProjektService.Object);
            string validId = "1"; // Valid Id

            var project = new Project();
            // Mock the ProjektService to return a sample Project
            mockProjektService.Setup(service => service.GetProjekt(validId))
                .ReturnsAsync(new OkObjectResult(project));

            // Act
            var result = await controller.GetProjekt(validId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(project, okResult.Value);
        }
        
        [Fact]
        public async void CreateProjekt_ReturnsOk_WhenProjectIsNotNull()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new BrokerProjectController(mockProjektService.Object);
            var project = new CreateProjectRequest(); // Valid Project

            // Mock the ProjektService to return IActionResult (e.g., OkResult)
            mockProjektService.Setup(service => service.CreateProjekt(project))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await controller.CreateProjekt(project);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async void CreateProjekt_ReturnsBadRequest_WhenProjectIsNull()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new BrokerProjectController(mockProjektService.Object);
            CreateProjectRequest nullProject = null; // Project is null

            // Act
            var result = await controller.CreateProjekt(nullProject);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
        
        
    
    }
    
}
