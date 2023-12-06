using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broker.Controllers;
using Broker.Services;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Task = ClassLibrary_SEP3.Task;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;

namespace Broker_Test.Controller_Test
{
    public class SprintBacklogControllerTest
    {
        [Fact]
        public async void Get_ReturnsSprintBacklogs()
        {
            // Arrange
            var projectId = "ProjectId";
            var mockService = new Mock<ISprintBacklogService>();
            var sprintBacklogs = new List<SprintBacklog>
            {
                new SprintBacklog { ProjectId = projectId, SprintBacklogId = "1", Title = "Sprint 1" },
                new SprintBacklog { ProjectId = projectId, SprintBacklogId = "2", Title = "Sprint 2" }
            };
            mockService.Setup(service => service.GetSprintBacklogsAsync(projectId))
                .ReturnsAsync(new OkObjectResult(sprintBacklogs));

            var controller = new SprintBacklogController(mockService.Object);
            var actionResult = await controller.GetAllSprintBacklogs(projectId);
            
            Assert.NotNull(actionResult);
            var objectResult = Assert.IsType<OkObjectResult>(actionResult);
            var model = Assert.IsAssignableFrom<IEnumerable<SprintBacklog>>(objectResult.Value);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async void AddTaskToSprintBacklog_ReturnsValue()
        {
            var mockService = new Mock<ISprintBacklogService>();
            var controller = new SprintBacklogController(mockService.Object);
            var projectId = "1";
            var sprintBacklogId = "2";
            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = sprintBacklogId,
                Title = "Sample Sprint",
                CreatedAt= new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>()
            };
            var sprintId = "2";
            var task = new AddSprintTaskRequest()
            {
                SprintId = "2",
                Title = "Brush Alma"
            };
            mockService.Setup(service => service.AddTaskToSprintBacklogAsync(task))
                .ReturnsAsync(new OkObjectResult(expectedSprintBacklog));
            var result = await controller.AddTaskToSprintBacklog(task);
            Assert.NotNull(result);

        }
        
        [Fact]
        public async void Post_CreatesSprintBacklog2()
        {
            // Arrange
            var mockService = new Mock<ISprintBacklogService>();
            var controller = new SprintBacklogController(mockService.Object);

            var sprintBacklogData = new CreateSprintBackLogRequest
            {
                projectId = "sampleProjectId",
                Title = "Sample Sprint",
                Timestamp = new DateTime(2021, 1, 1),
            };

            mockService.Setup(service => service.CreateSprintBacklogAsync(It.IsAny<CreateSprintBackLogRequest>()))
                .ReturnsAsync(new OkObjectResult(sprintBacklogData));

            // Act
            var actionResult = await controller.Post(sprintBacklogData);

            // Assert
            mockService.Verify(service => service.CreateSprintBacklogAsync(It.IsAny<CreateSprintBackLogRequest>()), Times.Once);
            Assert.NotNull(actionResult);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult);
            Assert.Equal(201, createdAtActionResult.StatusCode);
        }
        [Fact]
        public async void GetSpecificSprintBacklog_ReturnsValue()
        {
            // Arrange
            var projectId = "ProjectId";
            var sprintBacklogId = "5";
            var mockService = new Mock<ISprintBacklogService>();
            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = sprintBacklogId,
                Title = "Sample Sprint",
                CreatedAt= new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>()
            };
            
            mockService.Setup(service => service.GetSprintBacklogByIdAsync(projectId, sprintBacklogId))
                .ReturnsAsync(new OkObjectResult(expectedSprintBacklog));

            var controller = new SprintBacklogController(mockService.Object);

         
            var result = await controller.GetSpecificSprintBacklog(projectId, sprintBacklogId);
            
            Assert.NotNull(result);
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var returnedValue = Assert.IsType<SprintBacklog>(objectResult.Value);
            Assert.Equal(expectedSprintBacklog, returnedValue); // Adjust the expected value according to your requirements
        }

        [Fact]
        public async void AddTaskToSprintBacklogValid()
        {
            var projectId = "1";
            var sprintBacklogId = "5";
            var mockService = new Mock<ISprintBacklogService>();
            var addTaskRequest = new AddSprintTaskRequest
                        {
                            ProjectId = "1",
                            SprintId = "5",
                            Title = "Implement methods",
                            Description = "Do it",
                            Status = TaskStatus.ToDo, 
                            CreatedAt = DateTime.Now,
                            EstimateTimeInMinutes = 120,
                            ActualTimeUsedInMinutes = 0,
                            Responsible = "Tom Riddle"
                        };
            var mockTask = new Task
            {
                Id = "1",
                ProjectId = addTaskRequest.ProjectId,
                SprintId = addTaskRequest.SprintId,
                Title = addTaskRequest.Title,
                Description = addTaskRequest.Description,
                Status = addTaskRequest.Status,
                CreatedAt = addTaskRequest.CreatedAt,
                EstimateTimeInMinutes = addTaskRequest.EstimateTimeInMinutes,
                ActualTimeUsedInMinutes = addTaskRequest.ActualTimeUsedInMinutes,
                Responsible = addTaskRequest.Responsible
            };
            var expectedSprintBacklog = new SprintBacklog
            {
                ProjectId = projectId,
                SprintBacklogId = sprintBacklogId,
                Title = "Sample Sprint",
                CreatedAt= new DateTime(2021, 1, 1),
                Tasks = new List<ClassLibrary_SEP3.Task>{mockTask}
            };
            
            mockService.Setup(service => service.AddTaskToSprintBacklogAsync(It.IsAny<AddSprintTaskRequest>()))
                .ReturnsAsync(new OkObjectResult(expectedSprintBacklog));

            var controller = new SprintBacklogController(mockService.Object);
            var result = await controller.AddTaskToSprintBacklog(addTaskRequest);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var sprintBacklog = Assert.IsType<SprintBacklog>(okResult.Value);

            Assert.NotNull(result);
            Assert.Contains(sprintBacklog.Tasks, task => task.Title == mockTask.Title && task.Description == mockTask.Description);
        }

        [Fact]
        public async void GetAllTasksForSprintBacklog()
        {
            var projectId = "1";
            var sprintBacklogId = "4";
            var mockService = new Mock<ISprintBacklogService>();
            var expectedTask = new List<Task>
            {
                new Task { Id = "1", ProjectId = "1", SprintId = "2", Title = "Task", Description = "Try and code" },
                new Task { Id = "2", ProjectId = "1", SprintId = "2", Title = "Task 2", Description = "I dont know what to put here" },
            };
            mockService.Setup(service => service.GetTasksFromSprintBacklogAsync(projectId, sprintBacklogId))
                .ReturnsAsync(new OkObjectResult(expectedTask));
            var controller = new SprintBacklogController(mockService.Object);
            var result = await controller.GetAllTasksForSprintBacklog("1", "4");
           
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTasks = Assert.IsAssignableFrom<IEnumerable<Task>>(okResult.Value);
            foreach (var task in expectedTask)
            {
                var actualTask = returnedTasks.FirstOrDefault(t => t.Id == task.Id);
                Assert.NotNull(actualTask); // Ensure the task is found
                Assert.Equal(task.ProjectId, actualTask.ProjectId);
                Assert.Equal(task.SprintId, actualTask.SprintId);
                Assert.Equal(task.Title, actualTask.Title);
            }
        }
    }
}
