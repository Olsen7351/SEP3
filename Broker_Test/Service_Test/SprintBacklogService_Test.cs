using Broker.Services;
using ClassLibrary_SEP3;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using Task = ClassLibrary_SEP3.Task;
using TaskSys = System.Threading.Tasks.Task;


namespace Broker_Test.Service_Test
{
    public class SprintBacklogService_Test
    {
        
        [Fact]
        public async TaskSys CreateSprintBacklogAsync_ReturnsSuccess()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            var service = new SprintBacklogService(mockHttpClient.Object);

            var sprintBacklog = new SprintBacklog
            {
                ProjectId = "project-1",
                SprintBacklogId = "backlog-1",
                Title = "Sample Sprint",
                CreatedAt = DateTime.UtcNow,
                Tasks = new List<Task>()
            };

            // Act
            var result = await service.CreateSprintBacklogAsync(sprintBacklog);

            // Assert
            Assert.NotNull(result);
            //If the SprintBacklog is created successfully, the result should be an CreatedAtActionResult
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async TaskSys GetSprintBacklogsAsync_ReturnsListOfSprintBacklogs()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            var service = new SprintBacklogService(mockHttpClient.Object);
            var projectId = "sampleProjectId";

            // Setup HttpClient behavior for the GetSprintBacklogsAsync method
            var expectedSprintBacklogs = new List<SprintBacklog>
            {
                new SprintBacklog
                {
                    ProjectId = projectId,
                    SprintBacklogId = "backlog-1",
                    Title = "Sample Sprint 1",
                    CreatedAt = DateTime.UtcNow,
                    Tasks = new List<Task>()
                },
            };
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponse.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedSprintBacklogs));
            mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponse);

            // Act
            var result = await service.GetSprintBacklogsAsync(projectId);
            var json = result.ToJson();
            var sprintBacklogs = JsonConvert.DeserializeObject<List<SprintBacklog>>(json);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedSprintBacklogs, sprintBacklogs);
            Assert.Equal(expectedSprintBacklogs.Count, sprintBacklogs.Count);
        }

        [Fact]
        public async TaskSys GetSprintBacklogByIdAsync_ReturnsSpecificSprintBacklog()
        {
            // Arrange
            var mockHttpClient = new Mock<HttpClient>();
            var service = new SprintBacklogService(mockHttpClient.Object);
            var projectId = "sampleProjectId";
            var backlogId = "sampleBacklogId";

            // Setup HttpClient behavior for the GetSprintBacklogByIdAsync method
            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = backlogId,
                Title = "Sample Sprint",
                CreatedAt = DateTime.UtcNow,
                Tasks = new List<Task>()
            };
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK);
            httpResponse.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedSprintBacklog));
            mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponse);

            // Act
            var result = await service.GetSprintBacklogByIdAsync(projectId, backlogId);
            var jsonResult = result.ToJson();
            var sprintBacklog = JsonConvert.DeserializeObject<SprintBacklog>(jsonResult);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedSprintBacklog, sprintBacklog);
        }
        
    }
}
