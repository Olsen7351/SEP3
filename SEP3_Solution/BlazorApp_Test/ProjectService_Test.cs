using BlazorAppTEST.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
    {    // Arrange
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
        Assert.Equal(expectedProject.Description,result.Description);
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


    
    
    [Fact]
    public async Task Add()
    {
        
    }
    
    
}

