using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Task = System.Threading.Tasks.Task;

namespace Broker_Test.Service_Test

{
    public class ProjectServiceTest
    {
        private readonly HttpClient _httpClient;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly ProjectService _projectService;

        public ProjectServiceTest()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("http://mockserver/")
            };
            _projectService = new ProjectService(_httpClient);
        }

        [Fact]
        public async Task CreateProjekt_ValidProject_ReturnsOk()
        {
            var projectToCreate = new CreateProjectRequest()
            {
                Name = "Test Project",
                Description = "Test Project Description",
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 12, 31)
            };
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith("api/Project")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var result = await _projectService.CreateProjekt(projectToCreate);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task GetProjekt_ValidId_ReturnsProject()
        {
            string projectId = "123";
            var expectedProject = new Project
            {
                Name = "Test Project",
                Description = "Test Project Description",
                Id = "123",
                StartDate = new DateTime(2023, 1, 1),
                EndDate = new DateTime(2023, 12, 31)
            };
            string responseProjectJson = JsonSerializer.Serialize(expectedProject);
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseProjectJson, System.Text.Encoding.UTF8, "application/json")
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith($"api/Project/{projectId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);


            var result = await _projectService.GetProjekt(projectId);
            Assert.NotNull(result);
            var project = Assert.IsType<Project>(result);
            Assert.Equal(expectedProject.Id, project.Id);
            Assert.Equal(expectedProject.Description, project.Description);
            Assert.Equal(expectedProject.Name, project.Name);

        }

        [Fact]
        public async Task GetMembersForProject()
        {
            string projectId = "123";
            var expectedMembers = new List<string>
            {
                "123",
                "456"
            };
            string responseMembersJson = JsonSerializer.Serialize(expectedMembers);
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseMembersJson, System.Text.Encoding.UTF8, "application/json")
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                                   "SendAsync",
                                                      ItExpr.Is<HttpRequestMessage>(req =>
                                                                                 req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith($"api/Project/{projectId}/Members")),
                                                      ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);
            var result = await _projectService.GetProjectMembers(projectId);
            Assert.NotNull(result);
            var members = Assert.IsType<List<string>>(result);
            Assert.Equal(expectedMembers.Count, members.Count);
        }

        [Fact]
        public async Task GetErrorIfNoMembersInProject()
        {
            string projectId = "123";
            var noMembers = new List<string>();
            string responseMembersJson = JsonSerializer.Serialize(noMembers);
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(responseMembersJson, System.Text.Encoding.UTF8, "application/json")
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                                                  "SendAsync",
                                                                                                       ItExpr.Is<HttpRequestMessage>(req =>
                                                                                                                                                                                           req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith($"api/Project/{projectId}/Members")),
                                                                                                       ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            //Check if the method throws an NullReferenceException
            await Assert.ThrowsAsync<NullReferenceException>(() => _projectService.GetProjectMembers(projectId));
        }

        [Fact]
        public async Task AddUserToProject_ValidRequest_ReturnsOk()
        {
          
            var addUserToProjectRequest = new AddUserToProjectRequest
            {
                ProjectId = "123",
                Username = "456"
            };
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().EndsWith("api/Broker/AddUserToProject")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);
            var result = await _projectService.AddUserToProject(addUserToProjectRequest);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddUserToProject_NullRequest_ReturnsBadRequest()
        {
            AddUserToProjectRequest addUserToProjectRequest = null;
            var result = await _projectService.AddUserToProject(addUserToProjectRequest);
            Assert.IsType<BadRequestResult>(result);
            
        }
        
    }
}