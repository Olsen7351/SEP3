using System.Security.Claims;
using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Http;
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
            var mockProjektService = new Mock<IProjectService>();
            var controller = new BrokerProjectController(mockProjektService.Object);

            var verifiedUsername = "Alma";
            var projectRequest = new CreateProjectRequest
            {
                Name = "Sample Project", 
                Description = "A description of the Sample Project", 
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),
                ByUsername = verifiedUsername 
            };
            
            mockProjektService.Setup(service => service.CreateProjekt(It.IsAny<CreateProjectRequest>()))
                .ReturnsAsync(new OkResult());
            
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, verifiedUsername) };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext
            };

            // Act
            var result = await controller.CreateProjekt(projectRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
        }
        
        [Fact]
        public async void CreateProjekt_ReturnsBadRequest_WhenProjectIsNull()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new BrokerProjectController(mockProjektService.Object);

            // Mock HttpContext for Authorization header
            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.Request.Headers["Authorization"] = "Bearer testtoken";
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext
            };

            // Act
            var result = await controller.CreateProjekt(null); // Passing null directly

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
    

