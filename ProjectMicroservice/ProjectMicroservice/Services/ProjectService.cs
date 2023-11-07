using System.Collections.Concurrent;
using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;
using ZstdSharp;

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
            catch (System.FormatException)
            {
                Console.WriteLine("Could Not find project");
                throw new Exception("Project not found");
            }
        }

        public bool ProjectExists(ObjectId id)
        {
            try
            {
                return _projects.CountDocuments(b => b.Id == id) > 0;
            }
            catch (Exception) { return false; }
        }
        public ProjectDatabase UpdateProject(ProjectDatabase project)
        {
            try {
                var filter = Builders<ProjectDatabase>.Filter.Eq(p => p.Id, project.Id);

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
    }
}
