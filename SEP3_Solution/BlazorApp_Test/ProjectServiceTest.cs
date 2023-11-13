using System.ComponentModel.DataAnnotations;
using BlazorAppTEST.Services;
using ProjectMicroservice.DataTransferObjects;

namespace BlazorAppTest;


public class ProjectServiceTest
{

    private readonly HttpClient httpClient;
    private readonly ProjectService projectService;

    public ProjectServiceTest(HttpClient httpClient, ProjectService projectService)
    {
        this.httpClient = httpClient;
        this.projectService = projectService;
    }
    [Fact]
    public async Task CreateProject_Successful()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "This is a test project.",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7),
        };

        // Act
        await projectService.CreateProject(createProjectRequest);

        // Assert
        // If no exception is thrown, the test passes
    }

    [Fact]
    public async Task GetProject_ExistingId_ReturnsProject()
    {
        var projectId = "1";
        //Act
        var result = await projectService.GetProject(projectId);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal("ExpectedProjectName",result.Name);
    }
    
    [Fact]
    public async Task GetProject_NonexistentId_ThrowsException()
    {
        // Arrange
        var projectId = "999"; 

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => projectService.GetProject(projectId));
    }
    [Fact]
    public void CreateProjectRequest_Validation_Success()
    {
        // Arrange
        var createProjectRequest = new CreateProjectRequest
        {
            Name = "Test Project",
            Description = "This is a test project.",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(7),
           
        };

        // Act
        var validationResult = ValidateModel(createProjectRequest);

        // Assert
        Assert.Empty(validationResult); // No validation errors should be present
    }
    public void Dispose()
    {
        // Clean up resources, if any
        httpClient.Dispose();
    }
    
    
    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(model, serviceProvider: null, items: null);
        Validator.TryValidateObject(model, context, validationResults, validateAllProperties: true);
        return validationResults;
    }
    
    /*Denne form for valideringsmetode er nyttig, når du ønsker at udføre validering på et objekt uden at kaste en exception for fejl.
     I stedet kan du inspicere validationResults for at afgøre, hvilke valideringsfejl der er opstået,
      og tage passende handlinger baseret på dem*/
}