using System.Net;
using System.Text;
using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using ProjectMicroservice.DataTransferObjects;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTest;

public class ProjectService_Test
{
    private Mock<IProjectService> _mockService;
    private ProjectService _service;

    public ProjectService_Test()
    {
        _mockService = new Mock<IProjectService>();
        _service = new ProjectService(new HttpClient());
    }

    [Fact]
    public async Task ValidInput_ReturnsOkResult()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest();

        _mockService.Setup(service => service.CreateProject(createProjectRequest))
            .ReturnsAsync((ActionResult)new OkResult());

        // Act
        var result = await _mockService.Object.CreateProject(createProjectRequest) as OkResult;

        // Assert
        _mockService.Verify(service => service.CreateProject(createProjectRequest), Times.Once);


        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }



    [Fact]
    public async Task ValidInput_ReturnsOkObjectResult()
    {
        // Arrange
        var projectId = "sampleProjectId";


        var expectedProject = new Project
        {
            Id = "1",
            Name = "Sample Project",
            Description = "ValidDescription"
        };

        _mockService.Setup(service => service.GetProject(projectId))
            .ReturnsAsync(expectedProject);

        // Act
        var result = await _mockService.Object.GetProject(projectId) as Project;


        Assert.NotNull(result);
        Assert.Equal(expectedProject.Id, result.Id);
        Assert.Equal(expectedProject.Name, result.Name);
        Assert.Equal(expectedProject.Description, result.Description);
    }

    [Fact]
    public async Task NonExistingId_ReturnsNotFound()
    {
        // Arrange
        var nonExistingProjectId = "nonExistingId";

        _mockService.Setup(service => service.GetProject(nonExistingProjectId))
            .ReturnsAsync((Project)null);

        // Act
        var result = await _mockService.Object.GetProject(nonExistingProjectId);

        // Assert
        Assert.Null(result);

    }




    //---------------------------------------------------------------------- Adding a user into a project Tests

    [Fact]
    public async Task AddUserInsideProject_DoesNotRaiseException()
    {
        // Arrange
        string username = "James";
        string nonExistingProjectId = "jbjhasbfjhsabf";

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        var emptyProjectJson = "{}"; // Minimal valid JSON, adjust if the method expects a specific structure

        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(emptyProjectJson, Encoding.UTF8, "application/json")
            });

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var projectService = new ProjectService(httpClient);

        // Act
        Func<Task> act = async () => await projectService.AddUserToProject(username, nonExistingProjectId);

        // Assert
        await act(); 
    }



    //---------------------------------------------------------------------- Adding into project with username
    [Fact]
    public async Task AddUserInsideProjectWithNullUsername()
    {
        // Arrange
        string username = null;
        string nonExistingProjectId = "2";

        
        //Black Magic
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var projectService = new ProjectService(httpClient);


        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => projectService.AddUserToProject(username, nonExistingProjectId));
        Assert.Contains("Either username or projectID couldn't be retrieved", exception.Message);
    }




    [Fact]
    public async Task AddUserInsideProjectWithEmptyUsername()
    {
        // Arrange
        string username = "";
        string nonExistingProjectId = "2";

        
        //Black Magic
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var projectService = new ProjectService(httpClient);


        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => projectService.AddUserToProject(username, nonExistingProjectId));
        Assert.Contains("Either username or projectID couldn't be retrieved", exception.Message);
    }



    //---------------------------------------------------------------------- Adding into project with projectID
    [Fact]
    public async Task AddUserInsideProjectWithNullProjectID()
    {
        // Arrange
        string username = "James";
        string nonExistingProjectId = null;

        
        //Black Magic
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var projectService = new ProjectService(httpClient);


        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => projectService.AddUserToProject(username, nonExistingProjectId));
        Assert.Contains("Either username or projectID couldn't be retrieved", exception.Message);
    }




    [Fact]
    public async Task AddUserInsideProjectWithEmptyProjectID()
    {
        // Arrange
        string username = null;
        string nonExistingProjectId = "";

        
        //Black Magic
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)); 

        var httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("http://localhost/") 
        };
        var projectService = new ProjectService(httpClient);


        // Act
        var exception = await Assert.ThrowsAsync<Exception>(() => projectService.AddUserToProject(username, nonExistingProjectId));
        Assert.Contains("Either username or projectID couldn't be retrieved", exception.Message);
    }
}
