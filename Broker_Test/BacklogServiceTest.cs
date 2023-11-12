using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Broker.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using ProjectMicroservice.Models;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;

namespace Broker_Test
{
    public class BacklogServiceTest
    {
        [Fact]
        public async Task GetBacklog_ValidProjectId()
        {
            var projectId = 1;
            var backlog = new Backlog();

            var handleMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(backlog), Encoding.UTF8)
            };
            handleMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().EndsWith($"api/Backlog/{projectId}")),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            var httpclient = new HttpClient(handleMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5172/")
            };
            var backlogService = new BacklogService(httpclient);
            var resultat = await backlogService.GetBacklog(projectId);
            var resultat2 = Assert.IsType<OkObjectResult>(resultat);
            Assert.NotNull(resultat2.Value);
            Assert.IsType<Backlog>(resultat2.Value);

        }

        [Fact]
        public async Task CreateBacklog()
        {

            var backlogToCreate = new Backlog();
            var createdBacklog = new Backlog();
            var handleMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = new StringContent(JsonConvert.SerializeObject(createdBacklog), Encoding.UTF8)
            };
            
            handleMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",  ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith("api/Backlog")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpclient = new HttpClient(handleMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5172/")
            };
            var backlogService = new BacklogService(httpclient);
            var resultat = await backlogService.CreateBacklog(backlogToCreate);
            var resultat2 = Assert.IsType<OkObjectResult>(resultat);
            var returnBacklog = Assert.IsType<Backlog>(resultat2.Value);
            Assert.NotNull(returnBacklog);
        }

        [Fact]
        public async Task AddTaskToBacklog()
        {
            var projectId = 1;
            var taskToAdd = new ClassLibrary_SEP3.Task
            {
                Title = "Pet Alma",
                Description = "This is a daily task",
                Status = TaskStatus.ToDo,
                CreatedAt = DateTime.Now,
                EstimateTime = DateTime.UtcNow.AddDays(1),
                Responsible = "Nini"
            };
            var handleMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(taskToAdd), Encoding.UTF8)
            };
            handleMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req 
                => req.Method == HttpMethod.Post && req.RequestUri.ToString().EndsWith($"api/Project/{projectId}/Backlog/Task")),
            ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);

            var httpClient = new HttpClient(handleMock.Object)
            {
                BaseAddress = new Uri("http://localhost:5172/")
            };
            var backlogService = new BacklogService(httpClient);
            var resultat = await backlogService.AddTaskToBackLog(projectId, taskToAdd);
            var resultat2 = Assert.IsType<OkObjectResult>(resultat);
            Assert.NotNull(resultat2.Value);
        }
        
    }
}

