using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.Services;
using Xunit;

namespace ProjectMicroservice_Tests.DAO_Services
{
    public class ProjectServiceTests : IClassFixture<MongoDbFixture>
    {
        /*#####MAKE SURE THE TEST DATEBASE IS RUNNING & EMPTY BEFORE RUNNING THE TESTS#####*/
        private readonly IMongoCollection<Project> _testProjects;
        private readonly IMongoCollection<UsersAPartOfProjects> _testUsers;
        private readonly ProjectService _projectService;

        public ProjectServiceTests(MongoDbFixture fixture)
        {
            _testProjects = fixture.Database.GetCollection<Project>("Projects");
            _projectService = new ProjectService(new MongoDbContext(fixture.ConnectionString,"test_db"));
            _testUsers = fixture.Database.GetCollection<UsersAPartOfProjects>("ProjectsForUsers");
        }

        [Fact]
        public void CreateProject_ReturnsNewProject()
        {
            // Arrange
            var createRequest = new CreateProjectRequest
            {
                Name = "Test Project",
                Description = "Test Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30)
            };

            // Act
            var result = _projectService.CreateProject(createRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(createRequest.Name, result.Name);
            // Add more assertions based on your Project structure and expected data
        }

        [Fact]
        public void GetProject_ProjectExists_ReturnsProject()
        {
            // Arrange
            var project = new Project
            {
                Name = "Existing Project",
                Description = "Existing Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10)
            };

            _testProjects.InsertOne(project);

            // Act
            var result = _projectService.GetProject(project.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project.Id, result.Id);
            // Add more assertions based on your Project structure and expected data
        }

        [Fact]
        public void ProjectExists_ProjectExists_ReturnsTrue()
        {
            // Arrange
            var project = new Project
            {
                Name = "Existing Project",
                Description = "Existing Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10)
            };

            _testProjects.InsertOne(project);

            // Act
            var result = _projectService.ProjectExists(project.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void UpdateProject_ProjectExists_UpdatesProject()
        {
            // Arrange
            var project = new Project
            {
                Name = "Existing Project",
                Description = "Existing Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(10)
            };

            _testProjects.InsertOne(project);

            var updatedProject = project;
            updatedProject.Description = "Updated Description";

            // Act
            var result = _projectService.UpdateProject(updatedProject);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedProject.Description, result.Description);
            // Add more assertions based on your Project structure and expected data
        }

        [Fact]
        public void AddUserToProject_validRequest_ReturnTrue()
        {
            var createProjectRequest = new CreateProjectRequest
            {
                Name = "Test Project",
                Description = "Test Description",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30)
            };
            var project = _projectService.CreateProject(createProjectRequest);

            var addUserToProjectRequest = new AddUserToProjectRequest()
            {
                ProjectId = project.Id,
                Username = "TestUser"
            };
            var result = _projectService.AddUserToProject(addUserToProjectRequest);
            Assert.True(result);

            var userPartOfProject = _testUsers.Find(u => u.Username == "TestUser").FirstOrDefault();
            Assert.NotNull(userPartOfProject);
            Assert.Contains(project.Id, userPartOfProject.ProjectID);
        }
    }

    public class MongoDbFixture
    {
        public IMongoDatabase Database { get; private set; }
        public string ConnectionString { get; private set; }

        public MongoDbFixture()
        {
            // Load configuration from appsettings.json
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string from configuration
            ConnectionString = config.GetConnectionString("MongoDb");

            // Initialize the database with the connection string
            var client = new MongoClient(ConnectionString);
            Database = client.GetDatabase("test_db"); // Replace with your test database name
        }
    }
}
