using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;

namespace ProjectMicroservice_Tests.DAO_Services
{
    /*#####MAKE SURE THE TEST DATEBASE IS RUNNING & EMPTY BEFORE RUNNING THE TESTS#####*/
    public class SprintServiceTests
    {
        private readonly MongoDbContext _dbContext;
        private readonly SprintService _sprintService;
        private string _sprintBacklogId;
        private readonly ProjectService _projectService;

        public SprintServiceTests()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = configBuilder.Build();
            var connectionString = configuration.GetConnectionString("MongoDb");
            var databaseName = "test_db"; // Update with your database name

            _dbContext = new MongoDbContext(connectionString, databaseName);
            _sprintService = new SprintService(_dbContext);
            _projectService = new ProjectService(_dbContext);
        }

        [Fact]
        public void CreateSprintBacklog_SprintBacklogNotExists_CreatesNewSprintBacklog()
        {
            // Arrange
            var request = new CreateSprintBackLogRequest
            {
                projectId = "project_id_1",
                Title = "Test Sprint",
                // Add more properties needed for your request
            };

            // Act
            var result = _sprintService.CreateSprintBacklog(request);

            _sprintBacklogId = result.SprintBacklogId;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.projectId, result.ProjectId);
            Assert.Equal(request.Title, result.Title);
            // Add more assertions as needed
        }

        [Fact]
        public void GetSprintBacklogById_SprintBacklogExists_ReturnsSprintBacklog()
        {
            // Arrange
            var request = new CreateSprintBackLogRequest
            {
                projectId = "project_id_12",
                Title = "Test Sprint",
                // Add more properties needed for your request
            };

            var createdSprint = _sprintService.CreateSprintBacklog(request);
            var sprintBacklogId = createdSprint.SprintBacklogId; // Retrieve the ID of the newly created Sprint

            // Act
            var result = _sprintService.GetSprintBacklogById(sprintBacklogId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(sprintBacklogId, result.SprintBacklogId);
            Assert.Equal(request.projectId, result.ProjectId);
            Assert.Equal(request.Title, result.Title);
        }


        [Fact]
        public void DeleteSprintBacklog_SprintBacklogExists_DeletesSprintBacklog()
        {
            var projectRequest = new CreateProjectRequest
            {
                Name = "Test Project",
                Description = "This is a test project for my cat.",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ByUsername = "Alma"
            };
            var createdProject = _projectService.CreateProject(projectRequest);
            var sprintRequest = new CreateSprintBackLogRequest
            {
                projectId = createdProject.Id, 
                Title = "Test Sprint",
                Timestamp = DateTime.Now,
                Deadline = DateTime.Now.AddDays(14)
            };
            var createdSprint = _sprintService.CreateSprintBacklog(sprintRequest);
            var deleteResult = _sprintService.DeleteSprintBacklog(createdProject.Id, createdSprint.SprintBacklogId);
            Assert.True(deleteResult);
            
        }
        [Fact]
        public void GetAllSprintBacklogs_ForProject_ReturnsAllSprints()
        {
            var projectRequest = new CreateProjectRequest
            {
                Name = "Test Project",
                Description = "This is a test project for my cat.",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(1),
                ByUsername = "Alma"
            };
            var project = _projectService.CreateProject(projectRequest); 
            var sprintRequest1 = new CreateSprintBackLogRequest { projectId = project.Id, Title = "Alma"  };
            var sprintRequest2 = new CreateSprintBackLogRequest { projectId = project.Id, Title = "Paimon" };
            _sprintService.CreateSprintBacklog(sprintRequest1); 
            _sprintService.CreateSprintBacklog(sprintRequest2); 

            var sprintBacklogs = _sprintService.GetAllSprintBacklogs(project.Id);

            Assert.NotNull(sprintBacklogs);
            Assert.Equal(2, sprintBacklogs.Count()); 
        }
    }
}