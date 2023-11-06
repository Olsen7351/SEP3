using System.Collections.Concurrent;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMongoCollection<ProjectDatabase> _projects;

        public ProjectService(MongoDbContext context)
        {
            _projects = context.Database.GetCollection<ProjectDatabase>("Projects");
        }

        public ProjectDatabase CreateProject(CreateProjectRequest request) 
        {
            var newProject = new ProjectDatabase
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Backlog = new Backlog()
            };

            _projects.InsertOne(newProject);
            return newProject;  // Now contains the MongoDB-generated ID
        }

        public ProjectDatabase GetProject(ObjectId id)
        {
            try
            {
                return _projects.Find(p => p.Id == id).FirstOrDefault();
            }
            catch (System.FormatException) { return null; }
        }

        public bool ProjectExists(ObjectId id)
        {
            try
            {
                return _projects.CountDocuments(b => b.Id == id) > 0;
            }
            catch (Exception) { return false; }
        }
    }
}
