using System.Collections.Concurrent;
using System.Data.Common;
using ClassLibrary_SEP3;

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

    private readonly IMongoCollection<SprintBacklog> _sprints;

    public SprintService(MongoDbContext context)
    {
        _sprints = context.Database.GetCollection<SprintBacklog>("Sprints");
    }
    
    public SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request)
    {
        var filter =
            Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogID, request.Id);
        var sprintBacklog = _sprints.Find(filter).FirstOrDefault();

        if (sprintBacklog == null)
        {
            sprintBacklog = new SprintBacklog()
            {
                ProjectID = request.projectId,
                SprintBacklogID = request.Id,
                Title = request.Title,
                CreatedAt = DateTime.Today,
                Tasks = new List<Task>()
            };
            _sprints.InsertOne(sprintBacklog);
        }
        else
        {
            var update = Builders<SprintBacklog>.Update.Set(sprint => sprint.Title, request.Title);
            _sprints.UpdateOne(filter, update);        }

        return sprintBacklog;

    } 
    public SprintBacklog GetSprintBacklogById(string projectId,string sprintBacklogId)
    {
        try
        {
            return _sprints.Find(p => p.ProjectID == projectId && p.SprintBacklogID == sprintBacklogId)
                .FirstOrDefault();
        }catch (System.FormatException)
        {
            Console.WriteLine($"Could Not find Sprintbacklog {sprintBacklogId} for project {projectId}");
            throw new Exception("Project not found");
        }
    }
    public List<SprintBacklog> GetAllSprintBacklogs(string projectId)
    {
        return _sprints.Find(sprint => sprint.ProjectID == projectId).ToList();
    }



    public SprintBacklog UpdateSprintBacklog(string id, SprintBacklog updatedSprintBacklog)
    {
        var filter = Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogID, id);
        var update = Builders<SprintBacklog>.Update
            .Set(sprint => sprint.ProjectID, updatedSprintBacklog.ProjectID)
            .Set(sprint => sprint.Title, updatedSprintBacklog.Title);
        update = update.Set(sprint => sprint.Tasks, updatedSprintBacklog.Tasks);
        _sprints.UpdateOne(filter, update);
        return _sprints.Find(filter).FirstOrDefault();

    }

    public bool DeleteSprintBacklog(string projectId, string sprintBacklogId)

    {
            var filter = Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogID, sprintBacklogId);
            var result = _sprints.DeleteOne(filter);
            return result.DeletedCount > 0;
    }

    public SprintBacklog AddTaskToSprintBacklog(AddSprintTaskRequest request, string sprintBacklogId)
    {
        Task newTask = new Task
        {
            ProjectId = request.ProjectId,
            SprintId = request.SprintId,
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            CreatedAt = request.CreatedAt,
            EstimateTimeInMinutes = request.EstimateTimeInMinutes,
            ActualTimeUsedInMinutes = request.ActualTimeUsedInMinutes,
            Responsible = request.Responsible
        };
        var filter = Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogID, sprintBacklogId);
        var update = Builders<SprintBacklog>.Update.Push(sprint => sprint.Tasks, newTask);
        _sprints.UpdateOne(filter, update);
        return _sprints.Find(filter).FirstOrDefault();
    }

    public List<Task> GetAllTasksForSprintBacklog(string projectId, string sprintBacklogId)
    {
        var filter = Builders<SprintBacklog>.Filter.Where(sprint => sprint.ProjectID == projectId && sprint.SprintBacklogID == sprintBacklogId);
        var sprintBacklog = _sprints.Find(filter).FirstOrDefault();
        if (sprintBacklog != null)
        {
            return sprintBacklog.Tasks;
        }
        return new List<Task>();
    }

    
    
}