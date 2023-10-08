using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using MainWeb.Services;
using Moq;
using Moq.Protected;
using ProjectMicroservice.Models;
using Xunit;

namespace BlazorAppTest
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task CreateProject_HTTP_POST()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected().Setup<Task<HttpResponseMessage>>
                (
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}"),
                });

            var client = new HttpClient(mockHandler.Object);
            var service = new ProjectService(client);
            var project = new Project
            {
                Id = 1,
                Name = "Test Project",
                Description = "This is a test project for unit testing.",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30)
            };

            // Act
            await service.CreateProjekt(project);
            
            
            // Assert
            mockHandler.Protected().Verify("SendAsync", Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}