using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = ClassLibrary_SEP3.Task;

namespace Broker_Test.Controller_Test
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

            var project = new Project
            {
                Id = "1",
                Name = "Alma",
                Description = "Brush Alma",
            };
            mockProjektService.Setup(service => service.GetProjekt(validId))
                .ReturnsAsync(project);

            // Act
            var result = await controller.GetProjekt(validId);

            var okResult = Assert.IsType<Project>(result);
            Assert.IsType<Project>(okResult);
            Assert.Equal(project, okResult);
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
        [Fact]
        public async void AddUserToProject_ReturnsOk_WhenRequestIsValid()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new BrokerProjectController(mockProjektService.Object);
            string validProjectId = "1";

            var request = new AddUserToProjectRequest
            {
                Username = "user123",
                ProjectId = "1"
            };

            mockProjektService.Setup(
                service => service.AddUserToProject(request)
            ).ReturnsAsync(new OkResult());

            // Act
            var result = await controller.AddUserToProject(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(okResult); 
           // Assert.Equal("Some content", okResult.Value.ToString()); // Corrected line
        }
        }
        
    
    }
    

