using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using DefaultNamespace;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectMicroservice.Controllers;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace ProjectMicroservice.Tests;

public class SprintControllerTest
{
    private readonly Mock<ISprintService> _mockSprintService;
    private readonly SprintController _sprintController;

    public SprintControllerTest()
    {
        _mockSprintService = new Mock<ISprintService>();
        _sprintController = new SprintController(_mockSprintService.Object);
    }

    [Fact]
    public async Task GetAllSprintBacklogs()
    {
        string projectId = "1";
        var sprintBacklogs = new List<SprintBacklog>
        {
            new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = "1",
                Title = "Sprint 1",
                CreatedAt = DateTime.Now,
            },
            new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = "2",
                Title = "Sprint 2",
                CreatedAt = DateTime.Now.AddDays(1),
            }
        };
        _mockSprintService.Setup(service => service.GetAllSprintBacklogs(projectId))
            .Returns(sprintBacklogs);


        var result = await _sprintController.GetAllSprintBacklogs(projectId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sprintBacklogs, okResult.Value);
    }

    [Fact]
    public async Task GetSpecificSprintBacklog()
    {
        string projectId = "1";
        string sprintBacklogId = "1";
        
        var sprintBacklog = new SprintBacklog
        {
            ProjectId = projectId,
            SprintBacklogId = sprintBacklogId,
            Title = "Sprint Backlog Title",
            CreatedAt = DateTime.Now
           
        };
        _mockSprintService.Setup(service => service.GetSprintBacklogById(sprintBacklogId)).Returns(sprintBacklog);
        var result = await _sprintController.GetSpecificSprintBacklog(projectId, sprintBacklogId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(sprintBacklog, okResult.Value);

    }

    [Fact]
    public async Task CreateSprintBacklog()
    {
        string projectId = "1";
        var request = new CreateSprintBackLogRequest
        {
            projectId = projectId,
            Id = "newBacklogId",
            Title = "New Sprint Backlog",
            Timestamp = DateTime.UtcNow,
        };
        var createdSprintBacklog = new SprintBacklog
        {
            ProjectId = projectId,
            SprintBacklogId = request.Id,
            Title = request.Title,
            CreatedAt = request.Timestamp
        };
        _mockSprintService.Setup(service => service.CreateSprintBacklog(request))
            .Returns(createdSprintBacklog);

        var result = await _sprintController.CreateSprintBacklog(projectId, request);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(createdSprintBacklog, createdResult.Value);
    }

    [Fact]
    public async Task UpdateSprintBacklog()
    {
        string projectId = "1";
        string sprintBacklogId = "1";
        
        var sprintBacklogToUpdate = new SprintBacklog
        {
            ProjectId = projectId,
            SprintBacklogId = sprintBacklogId,
            Title = "Updated Sprint Backlog",
        };
        var updatedSprintBacklog = new SprintBacklog
        {
            ProjectId = projectId,
            SprintBacklogId = sprintBacklogId,
            Title = "Updated Sprint Backlog - Confirmed",
        };
        _mockSprintService.Setup(service => service.UpdateSprintBacklog(sprintBacklogId, sprintBacklogToUpdate))
            .Returns(updatedSprintBacklog);

        var result = await _sprintController.UpdateSprintBacklog(projectId, sprintBacklogId, sprintBacklogToUpdate);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedSprintBacklog, okResult.Value);

    }

    [Fact]
    public async Task DeleteSprintBacklog()
    {
        string projectId = "1";
        string sprintBacklogId = "1";
        _mockSprintService.Setup(service => service.DeleteSprintBacklog(sprintBacklogId))
            .Returns(true);

        var result = await _sprintController.DeleteSprintBacklog(projectId, sprintBacklogId);
        var okResult = Assert.IsType<NoContent>(result);

    }
}

    


