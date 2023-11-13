using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using ProjectMicroservice.Models;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest
{
    public class ProjectServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        //Text conformation
        public ProjectServiceTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        
        
        //Create Project METHOD------------------------------------------------------------------------------------------------
        [Fact]
        public async Task CreateProject_HTTP_POST()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
    
            mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}"),
                });
            
            
            var client = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://testbaseaddress.com") // Setting the mock base address
            };
    
            var service = new ProjectService(client);
    
            var project = new Project
            {
                ProjectID = 1,
                Name = "Test Project",
                Description = "This is a test project for unit testing.",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30)
            };

            // Act
            await service.CreateProject(project);

            // Assert
            try
            {
                mockHandler.Protected().Verify("SendAsync", Times.Exactly(1),
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                );
                _testOutputHelper.WriteLine("HTTP_POST test succeeded");
            }
            catch
            {
                _testOutputHelper.WriteLine("HTTP_POST test failed");
                throw;
            }
        }



        
        //GetAllProjects METHOD------------------------------------------------------------------------------------------------
        [Fact]
        public async Task GetAllProjects_HTTP_GET()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            var mockProjects = new List<Project>
            {
                new Project
                {
                    ProjectID = 1, Name = "Test Project 1", Description = "test", StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                },
                new Project
                {
                    ProjectID = 2, Name = "Test Project 2", Description = "test", StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(30)
                }
            };

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(mockProjects)),
            };

            mockHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            ).ReturnsAsync(mockResponse);

            var client = new HttpClient(mockHandler.Object)
            {
                BaseAddress = new Uri("http://testbaseaddress.com")
            };
            var service = new ProjectService(client);


            // Act
            var projects = await service.GetAllProjects();

            // Assert
            Assert.Equal(2, projects.Count());
            mockHandler.Protected().Verify(
                "SendAsync", Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>()
            );
            _testOutputHelper.WriteLine("Get all projects succeeded.");
        }
    }
}