using System.Collections.Concurrent;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public class BacklogService : IBacklogService
    {
        private readonly IMongoCollection<Backlog> _backlogs;

        public BacklogService(MongoDbContext context)
        {
            _backlogs = context.Database.GetCollection<Backlog>("Backlogs");
        }

        public Backlog GetBacklogByProjectId(int projectId)
        {
            try
            {
                return _backlogs.Find(b => b.ProjectId == projectId).FirstOrDefault();
            }
            catch (System.FormatException) { return null; };
        }

        public Backlog CreateBacklog(int projectId, CreateBacklogRequest request)
        {
            var newBacklog = new Backlog
            {
                ProjectId = projectId,
                Description = request.Description
            };

            _backlogs.InsertOne(newBacklog);
            return newBacklog;  // Now contains the MongoDB-generated ID
        }

        public bool ProjectHasBacklog(int projectId)
        {
            try 
            {
                return _backlogs.CountDocuments(b => b.ProjectId == projectId) > 0;
            }
            catch (Exception) { return false; }
        }

    }
}
