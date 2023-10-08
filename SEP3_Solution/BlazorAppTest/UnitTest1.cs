using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using MainWeb.Services;
using Moq;
using Moq.Protected;
using Xunit;

namespace BlazorAppTest
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task CreateProjekt_ShouldPostSuccessfully()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>
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
                // fill in the properties here
            };

            // Act
            await service.CreateProjekt(project);

            // Assert
            // If no exceptions were thrown, the test will pass.
            // You can further assert that specific methods on the mock handler were called.
        }
    }
}