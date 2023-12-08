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

namespace ProjectMicroservice_Tests.DAO_Services
{
    /*#####MAKE SURE THE TEST DATEBASE IS RUNNING & EMPTY BEFORE RUNNING THE TESTS#####*/
    public class SprintServiceTests
    {
        private readonly MongoDbContext _dbContext;
        private readonly SprintService _sprintService;
        private string _sprintBacklogId;
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

            _sprintBacklogId= result.SprintBacklogId;
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

        /*
        [Fact]
        public void AddTaskToSprintBacklog()
        {
            var AddTask = 
        }
        */
    }
    


}
