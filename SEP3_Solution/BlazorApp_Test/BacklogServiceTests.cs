
using System.Net;

using Moq;
using Moq.Protected;

using BlazorAppTEST.Services;

using System.Text.Json;
using ClassLibrary_SEP3;
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
                Id = MongoDB.Bson.ObjectId.GenerateNewId(), // Generate a new ObjectId
                ProjectId = MongoDB.Bson.ObjectId.GenerateNewId(), // Example for ProjectId, use the appropriate value
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