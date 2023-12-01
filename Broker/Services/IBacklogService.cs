using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
=======
using ProjectMicroservice.DataTransferObjects;
>>>>>>> 88775a473578bea3f63a1222ee554c9e09398857
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Services;

public interface IBacklogService
{
    public Task<Task> AddTaskToBackLog(string projectId, AddBacklogTaskRequest? task);
    public Task<IActionResult> DeleteTaskFromBacklog(string id,string projectId);
}