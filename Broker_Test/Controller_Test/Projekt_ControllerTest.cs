/*using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
*/
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
            string validId = "1"; // Valid Id

            var project = new Project
            {
                Name = "Sample Project",
                Description = "A description of the Sample Project",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(30),

            };
            mockProjektService.Setup(service => service.GetProjekt(validId))
                .ReturnsAsync(project);

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma" for rabbitMQ
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            var controller = new BrokerProjectController(mockProjektService.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.GetProjekt(validId);

            var okResult = Assert.IsType<Project>(result);
            Assert.IsType<Project>(okResult);
            Assert.Equal(project, okResult);

            mockProjektService.Verify(service => service.GetProjekt(validId), Times.Once);
        }

        [Fact]
        public async void CreateProjekt_ReturnsOk_WhenProjectIsNotNull()
        {
            // Arrange
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

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma"
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);
            mockHttpContext.SetupGet(ctx => ctx.User).Returns(claimsPrincipal); // Adding the User property

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.CreateProjekt(projectRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

        }

        [Fact]
        public async void GetProjekt_ReturnsProject_WhenIdIsValid()
        {
            // Arrange
            var mockProjektService = new Mock<IProjectService>();
            var controller = new BrokerProjectController(mockProjektService.Object);
            string validId = "1"; // Valid Id

            var project = new Project
            {
                Id = "1",
                Name = "Sample Project",
                Description = "A description of the Sample Project",
            };
            mockProjektService.Setup(service => service.GetProjekt(validId))
                .ReturnsAsync(project);

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma"
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.GetProjekt(validId);

            // Assert
            Assert.NotNull(result);
            var projectResult = Assert.IsType<Project>(result);
            Assert.Equal(project, projectResult);

            // Verifying the service call
            mockProjektService.Verify(service => service.GetProjekt(validId), Times.Once);
        }
    

    [Fact]
        public async void CreateProjekt_ReturnsBadRequest_WhenProjectIsNull()
        {
                // Arrange
                var mockProjektService = new Mock<IProjectService>();
                var controller = new BrokerProjectController(mockProjektService.Object);

                // Mock HttpContext for Authorization header
                var mockHttpContext = new Mock<HttpContext>();
                var mockHttpRequest = new Mock<HttpRequest>();
                var mockHeaders = new HeaderDictionary();

                // Mock JWT Token with "sub" claim set to "Alma"
                var username = "Alma";
                var payload = new Dictionary<string, object> { { "sub", username } };
                var payloadJson = JsonSerializer.Serialize(payload);
                var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
                var mockJwtToken = $"header.{payloadBase64}.signature";

                mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
                mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
                mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

                controller.ControllerContext = new ControllerContext
                {
                    HttpContext = mockHttpContext.Object
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

            var mockHttpContext = new Mock<HttpContext>();
            var mockHttpRequest = new Mock<HttpRequest>();
            var mockHeaders = new HeaderDictionary();

            // Mock JWT Token with "sub" claim set to "Alma"
            var username = "Alma";
            var payload = new Dictionary<string, object> { { "sub", username } };
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(payloadJson));
            var mockJwtToken = $"header.{payloadBase64}.signature";

            mockHeaders["Authorization"] = "Bearer " + mockJwtToken;
            mockHttpRequest.Setup(r => r.Headers).Returns(mockHeaders);
            mockHttpContext.SetupGet(ctx => ctx.Request).Returns(mockHttpRequest.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };

            // Act
            var result = await controller.AddUserToProject(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.IsType<OkObjectResult>(okResult);
            // Assert.Equal("Some content", okResult.Value.ToString()); // Uncomment and correct as needed

            // Verifying the service call
            mockProjektService.Verify(service => service.AddUserToProject(request), Times.Once);
        }
    }
}
    

    

