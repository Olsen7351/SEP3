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
        
        var sprintBacklog = new SprintBacklog()
        {
                ProjectId = request.projectId,
                Title = request.Title,
                CreatedAt = DateTime.Today,
                Tasks = new List<Task>()
        };
        _sprints.InsertOne(sprintBacklog);
        
        return sprintBacklog;

    }
    public SprintBacklog GetSprintBacklogById(string sprintBacklogId)
    {
        try
        {
            var sprintBacklog = _sprints.Find(p => p.SprintBacklogId == sprintBacklogId)
                .FirstOrDefault();

            if (sprintBacklog == null)
            {
                Console.WriteLine($"Could not find Sprintbacklog {sprintBacklogId}");
                throw new NullReferenceException($"Sprintbacklog {sprintBacklogId}");
            }

            return sprintBacklog;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while retrieving sprint backlog: {ex.Message}");
            throw; // Re-throw the exception for higher-level handling or logging
        }
    }

    public List<SprintBacklog> GetAllSprintBacklogs(string projectId)
    {
        if (string.IsNullOrWhiteSpace(projectId))
        {
            throw new ArgumentException("Project ID cannot be null or whitespace.");
        }
        var sprintBacklogs = _sprints.Find(sprint => sprint.ProjectId == projectId).ToList();
        if (!sprintBacklogs.Any())
        {
            Console.WriteLine("There are no sprintbacklogs for this project");
            return null; 
        }

        return sprintBacklogs;
    }



    public SprintBacklog UpdateSprintBacklog(string id, SprintBacklog updatedSprintBacklog)
    {
        var filter = Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogId, id);
        var update = Builders<SprintBacklog>.Update
            .Set(sprint => sprint.ProjectId, updatedSprintBacklog.ProjectId)
            .Set(sprint => sprint.Title, updatedSprintBacklog.Title);
        update = update.Set(sprint => sprint.Tasks, updatedSprintBacklog.Tasks);
        _sprints.UpdateOne(filter, update);
        return _sprints.Find(filter).FirstOrDefault();

    }

    public bool DeleteSprintBacklog(string projectId, string sprintBacklogId)

    {
        Console.WriteLine("Microservice service Delete Sprint called");

        if (string.IsNullOrEmpty(sprintBacklogId))
        {
            Console.WriteLine("Sprint backlog ID is null or empty.");
            return false;
        }
        var sprintExistsFilter = Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogId, sprintBacklogId);
        var existingSprint = _sprints.Find(sprintExistsFilter).FirstOrDefault();

        if (existingSprint == null)
        {
            Console.WriteLine($"Sprint backlog with ID {sprintBacklogId} does not exist.");
            return false;
        }
        
        var deleteFilter = Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogId, sprintBacklogId);
        var result = _sprints.DeleteOne(deleteFilter);
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
            Deadline = request.Deadline,
            EstimateTimeInMinutes = request.EstimateTimeInMinutes,
            ActualTimeUsedInMinutes = request.ActualTimeUsedInMinutes,
            Responsible = request.Responsible
        };
        var filter = Builders<SprintBacklog>.Filter.Eq(sprint => sprint.SprintBacklogId, sprintBacklogId);
        var update = Builders<SprintBacklog>.Update.Push(sprint => sprint.Tasks, newTask);
        _sprints.UpdateOne(filter, update);
        return _sprints.Find(filter).FirstOrDefault();
    }

    public List<Task> GetAllTasksForSprintBacklog(string projectId, string sprintBacklogId)
    {
        var filter = Builders<SprintBacklog>.Filter.Where(sprint => sprint.ProjectId == projectId && sprint.SprintBacklogId == sprintBacklogId);
        var sprintBacklog = _sprints.Find(filter).FirstOrDefault();
        if (sprintBacklog != null)
        {
            return sprintBacklog.Tasks;
        }
        return new List<Task>();
    }

    
    
}