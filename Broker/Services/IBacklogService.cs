﻿using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;



using Task = ClassLibrary_SEP3.Task;

namespace Broker.Services;

public interface IBacklogService
{
    public Task<Task> AddTaskToBackLog(string projectId, AddBacklogTaskRequest? task);
    public Task<IActionResult> DeleteTaskFromBacklog(string id,string projectId);
}