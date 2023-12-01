using System.Collections.Concurrent;
using System.Data.Common;
using ClassLibrary_SEP3;
<<<<<<< Updated upstream

namespace ProjectMicroservice.Services;
=======
>>>>>>> Stashed changes
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.Services;
using ZstdSharp;
using Task = ClassLibrary_SEP3.Task;
namespace ProjectMicroservice.Services;

public class SprintService : ISprintService
{

    private readonly IMongoCollection<Project> _project;

    public SprintService(MongoDbContext context)
    {
        _project = context.Database.GetCollection<Project>("Projects");
    }
    
    public SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request)
    {
        var newSprintBackLog = new SprintBacklog()
        {
            ProjectId = request.projectId,
            SprintBacklogId = request.Id,
            Title = request.Title,
            CreatedAt = DateTime.Today,
            Tasks = new List<Task>()
        };
        _project.InsertOne();
    }
    public List<SprintBacklog> GetAllSprintBacklogs(string projectId)
    {
        throw new NotImplementedException();
    }
    

    public SprintBacklog GetSprintBacklogById(string sprintBacklogId)
    {
        throw new NotImplementedException();
    }



    public SprintBacklog UpdateSprintBacklog(string id, SprintBacklog sprintBacklog)
    {
        throw new NotImplementedException();
    }

    public bool DeleteSprintBacklog(string id)

    {
        throw new NotImplementedException();
    }

    public SprintBacklog AddTaskToSprintBacklog(Task task, string id, string sprintBacklogId)
    {
        throw new NotImplementedException();

    }

    public List<Task> GetAllTasksForSprintBacklog(string id, string sprintBacklogId)
    {
        throw new NotImplementedException();
    }
}