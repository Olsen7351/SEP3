using Broker.Controllers;
using Broker.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;
using ProjectMicroservice.Models;

namespace Broker_Test
{
    public class Projekt_Controller_Tests
    {
        [Fact]
        public void ProjektController_GetProjekt_ReturnsOk_WhenIdIsValid()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new ProjectController(mockProjektService.Object);
            int validId = 1; // Valid Id

            // Mock the ProjektService to return an OkObjectResult with a sample Project
            mockProjektService.Setup(service => service.GetProjekt(validId))
                .ReturnsAsync(new OkObjectResult(new Project()));

            // Act
            var result = controller.GetProjekt(validId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ProjektController_GetProjekt_ReturnsBadRequest_WhenIdIsNegative()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new ProjectController(mockProjektService.Object);
            int negativeId = -1; // Negative Id

            // Act
            var result = controller.GetProjekt(negativeId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void ProjektController_CreateProjekt_ReturnsOk_WhenProjectIsNotNull()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new ProjectController(mockProjektService.Object);
            var validProject = new Project(); // Valid Project
            validProject.Id = new ObjectId();
            validProject.Name = "Test";
            validProject.Description = "Test";
            validProject.StartDate = new DateTime(2021, 1, 1);
            validProject.EndDate = new DateTime(2021, 1, 2);

            // Mock the ProjektService to return an OkResult
            mockProjektService.Setup(service => service.CreateProjekt(validProject))
                .ReturnsAsync(new OkObjectResult(validProject));

            // Act
            var result = controller.CreateProjekt(validProject);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ProjektController_CreateProjekt_ReturnsBadRequest_WhenProjectIsNull()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new ProjectController(mockProjektService.Object);
            Project nullProject = null; // Project is null

            // Act
            var result = controller.CreateProjekt(nullProject);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
