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
        private readonly IMongoCollection<Project> _projects;

        public ProjectService(MongoDbContext context)
        {
            _projects = context.Database.GetCollection<Project>("Projects");
        }

        public Project CreateProject(CreateProjectRequest request)
        {
            var newProject = new Project
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            _projects.InsertOne(newProject);
            return newProject;  // Now contains the MongoDB-generated ID
        }

        public Project GetProject(ObjectId id)
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
