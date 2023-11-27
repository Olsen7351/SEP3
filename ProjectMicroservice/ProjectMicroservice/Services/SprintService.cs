

using ClassLibrary_SEP3;
using DefaultNamespace;
using ProjectMicroservice.Data;

namespace ProjectMicroservice.Services;
using ClassLibrary_SEP3.DataTransferObjects;
using MongoDB.Driver;

public class SprintService : ISprintService
{
    private readonly IMongoCollection<SprintBacklog> _sprintBacklogs;

    public SprintService(MongoDbContext context)
    {
        _sprintBacklogs = context.Database.GetCollection<SprintBacklog>("SprintBacklogs");
    }


    public List<SprintBacklog> GetAllSprintBacklogs(string projectId)
    {
        try
        {
            return _sprintBacklogs.Find(s => s.ProjectId == projectId).ToList();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            throw new Exception("Failed to retrieve sprint backlogs", ex);
        }
    }

    public SprintBacklog GetSprintBacklogById(string sprintBacklogId)
    {
        try
        {
            return _sprintBacklogs.Find(s => s.SprintBacklogId == sprintBacklogId).FirstOrDefault();
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            throw new Exception("Failed to retrieve sprint backlog by ID", ex);
        }
    }

    public SprintBacklog CreateSprintBacklog(CreateSprintBackLogRequest request)
    {
        try
        {
            var newSprintBacklog = new SprintBacklog()
            {
                ProjectId = request.projectId,
                Title = request.Title,
                CreatedAt = DateTime.Now,
                Tasks = request.Tasks
            };

            _sprintBacklogs.InsertOne(newSprintBacklog);
            return newSprintBacklog;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            throw new Exception("Failed to create sprint backlog", ex);
        }
    }

    public SprintBacklog UpdateSprintBacklog(string id, SprintBacklog sprintBacklog)
    {
        try
        {
            var filter = Builders<SprintBacklog>.Filter.Eq(s => s.SprintBacklogId, id);

            var result = _sprintBacklogs.ReplaceOne(filter, sprintBacklog);

            if (result.ModifiedCount == 1)
            {
                return sprintBacklog; // Successfully updated
            }
            else
            {
                throw new MongoException("Failed to update sprint backlog in database");
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            throw new Exception("Failed to update sprint backlog in database", ex);
        }
    }

    public bool DeleteSprintBacklog(string id)
    {
        try
        {
            var result = _sprintBacklogs.DeleteOne(s => s.SprintBacklogId == id);
            return result.DeletedCount == 1;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it accordingly
            throw new Exception("Failed to delete sprint backlog", ex);
        }
    }
}