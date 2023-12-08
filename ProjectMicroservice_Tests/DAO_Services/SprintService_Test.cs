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
    }
}

/*
[Fact]
public void AddTaskToSprint()
{
    var createProjectRequest = new CreateProjectRequest
    {
        Name = "Test Project",
        Description = "Description of Test Project",
        StartDate = DateTime.Now,
        EndDate = DateTime.Now.AddDays(30)
    };

    var createdProject = _projectService.CreateProject(createProjectRequest);
    string _projectId = createdProject.Id;

    var request = new CreateSprintBackLogRequest
    {
        projectId = _projectId,
        Title = "Test Sprint",

    };

    var createdSprint = _sprintService.CreateSprintBacklog(request);
    var sprintBacklogId = createdSprint.SprintBacklogId;
    var sprintTaskRequest = new AddSprintTaskRequest
    {
        ProjectId = _projectId,
        SprintId = sprintBacklogId,
        Title = "Test Task",
        Description = "Description of the test task",
        Status = TaskStatus.InProgress,
        CreatedAt = DateTime.Now,
        Deadline = DateTime.Now.AddDays(7),
        EstimateTimeInMinutes = 60,
        ActualTimeUsedInMinutes = 0,
        Responsible = "Responsible Person"
    };
    var result = _sprintService.AddTaskToSprintBacklog(sprintTaskRequest, sprintTaskRequest.SprintId);

    var addedTask = result.Tasks.FirstOrDefault(t => t.Title == sprintTaskRequest.Title);
    Assert.NotNull(addedTask);
    Assert.Equal(sprintTaskRequest.Title, addedTask.Title);
    Assert.Equal(sprintTaskRequest.Description, addedTask.Description);
}


}


}*/
