
using ClassLibrary_SEP3.DataTransferObjects;

namespace Broker_Test.Controller_Test;
using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Moq;

public class BackLogControllerTest
{
    [Fact]
    public async void AddTaskToBacklogValid()
    {
        
        var mockProject = new Mock<IBacklogService>();
        
        var controller = new BacklogController(mockProject.Object);
        var ValidBackLog = new AddBacklogTaskRequest
        {
            ProjectId = "1",
            Title = "Alma",
            Description = "Feed Alma",
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.Now
        };
        var expectedTask = new Task
        {
            Id = "task-123",
            ProjectId = ValidBackLog.ProjectId,
            Title = ValidBackLog.Title,
            Status = ValidBackLog.Status,
            CreatedAt = ValidBackLog.CreatedAt,
            EstimateTimeInMinutes = ValidBackLog.EstimateTimeInMinutes,
            ActualTimeUsedInMinutes = ValidBackLog.ActualTimeUsedInMinutes,
        };

        mockProject.Setup(service => service.AddTaskToBackLog(ValidBackLog.ProjectId, ValidBackLog))
            .ReturnsAsync(expectedTask);
        
        var result = await controller.AddTaskToBacklog(ValidBackLog.ProjectId, ValidBackLog);

        Assert.NotNull(result);
        var taskResult = Assert.IsType<Task>(result);
        Assert.Equal(expectedTask.Id, taskResult.Id);
        Assert.Equal(expectedTask.Title, taskResult.Title);
        Assert.Equal(expectedTask.Description, taskResult.Description);
        Assert.Equal(expectedTask.Status, taskResult.Status);
    }
    [Fact]
    public async void AddTaskToBacklogNotValid()
    {
        var mockBacklogService = new Mock<IBacklogService>();
        var controller = new BacklogController(mockBacklogService.Object);
        var validBackLogTaskRequest = new AddBacklogTaskRequest 
        {
            ProjectId = "1",
            Title = "Brush Alma",
            Description = "Use the brush on Alma",
            Status = TaskStatus.ToDo,
            CreatedAt = DateTime.Now,
            EstimateTimeInMinutes = 60,
            ActualTimeUsedInMinutes = 30,
            Responsible = "Nini"
        };

        mockBacklogService.Setup(service => service.AddTaskToBackLog(validBackLogTaskRequest.ProjectId, validBackLogTaskRequest))
            .ThrowsAsync(new Exception("Failed to add task"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => controller.AddTaskToBacklog(validBackLogTaskRequest.ProjectId, validBackLogTaskRequest));
        Assert.Equal("Failed to add task", exception.Message);
    }
    [Fact]
    public async void DeleteTaskFromBacklogValid()
    {
        // Arrange
        var mockBacklogService = new Mock<IBacklogService>();
        var controller = new BacklogController(mockBacklogService.Object);
        string validProjectId = "1";
        string validTaskId = "task-123";

        mockBacklogService.Setup(service => service.DeleteTaskFromBacklog(validTaskId, validProjectId))
            .ReturnsAsync(new OkResult());
        
        var result = await controller.DeleteTaskFromBacklog(validProjectId, validTaskId);
        
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async void DeleteTaskFromBacklogNotValid()
    {
        var mockBacklogService = new Mock<IBacklogService>();
        var controller = new BacklogController(mockBacklogService.Object);
        string validProjectId = "1";
        string invalidTaskId = "-1"; 

        mockBacklogService.Setup(service => service.DeleteTaskFromBacklog(invalidTaskId, validProjectId))
            .ReturnsAsync(new NotFoundResult());
        
        var result = await controller.DeleteTaskFromBacklog(validProjectId, invalidTaskId);
        Assert.IsType<NotFoundResult>(result);
    }
}