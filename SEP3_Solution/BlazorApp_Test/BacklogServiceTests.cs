using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Moq;
using Moq.Protected;
using Xunit;
using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using System.Text;
using System.Text.Json;
using ProjectMicroservice.Models;
using Task = System.Threading.Tasks.Task;

namespace YourNamespace.Tests
{
    public class BacklogServiceTests
    {
        [Fact]
        public async Task CreateBacklog_Should_SendHttpRequest()
        {
            // Arrange
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(""),
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response)
                .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            var backlogService = new BacklogService(httpClient);

            
            Backlog backlog = new Backlog
            {
                BacklogID = 1,
                ProjectID = 1
            };

            // Act
            await backlogService.CreateBacklog(backlog);

            // Assert
            handlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // Ensure that SendAsync was called exactly once
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post
                    && req.RequestUri.ToString().EndsWith("/api/Backlog")
                    && req.Content.ReadAsStringAsync().Result == JsonSerializer.Serialize(backlog,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}