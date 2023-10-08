using System.Collections.Concurrent;
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

        public Project GetProject(string id)
        {
            int parsedId;
            if (int.TryParse(id, out parsedId))
            {
                return _projects.Find(p => p.Id == parsedId).FirstOrDefault();
            }
            return null;
        }

        public bool ProjectExists(int projectId)
        {
            return _projects.CountDocuments(p => p.Id == projectId) > 0;
        }
    }
}
