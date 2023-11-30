using System.Collections.Concurrent;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.DataTransferObjects;
using ZstdSharp;
using Task = ClassLibrary_SEP3.Task;

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
            var newProject = new Project()
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Backlog = new Backlog()
                {
                    BacklogTasks = new List<Task>()
                }
            };

            _projects.InsertOne(newProject);
            return newProject;  // Now contains the MongoDB-generated ID
        }

        public Project GetProject(string id)
        {
            try
            {
                return _projects.Find(p => p.Id == id).FirstOrDefault();
            }
            catch (System.FormatException)
            {
                Console.WriteLine("Could Not find project");
                throw new Exception("Project not found");
            }
        }

        public bool ProjectExists(string id)
        {
            try
            {
                return _projects.CountDocuments(b => b.Id == id) > 0;
            }
            catch (Exception) { return false; }
        }
        public Project UpdateProject(Project project)
        {
            try {
                var filter = Builders<Project>.Filter.Eq(p => p.Id, project.Id);

                var result = _projects.ReplaceOne(filter, project);

                if (result.ModifiedCount == 1)
                {
                    return project; // Successfully updated
                }
                else
                {
                    throw new MongoException("Failed to update project in database"); // Update failed
                }
                return project;
            }
            catch (Exception)
            {
                throw new Exception("Failed to update project in database");
            }
        }

        public bool AddUserToProject(string projectId, string userName)
        {
            throw new NotImplementedException();
        }
    }
}
