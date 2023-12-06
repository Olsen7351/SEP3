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
            var projectId = "project_id_1";
            var sprintBacklogId = "1";

            // Act
            var result = _sprintService.GetSprintBacklogById(projectId, sprintBacklogId);
            Console.WriteLine(result);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(sprintBacklogId, result.SprintBacklogId);
            Assert.Equal(projectId, result.ProjectId);
            // Add more assertions based on your SprintBacklog structure and expected data
        }
    }
    

}
