using System.Collections.Concurrent;
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using Task = ClassLibrary_SEP3.Task;

namespace ProjectMicroservice.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IMongoCollection<Project> _projects;
        private readonly IMongoCollection<User> _user;

        public ProjectService(MongoDbContext context)
        {
            _projects = context.Database.GetCollection<Project>("Projects");
            _user = context.Database.GetCollection<User>("Users");
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
            return newProject; // Now contains the MongoDB-generated ID
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
            catch (Exception)
            {
                return false;
            }
        }
        
        //Alexanders method
        public IEnumerable<Project> GetProjectsByUser(string userId)
        {
            var filter = Builders<Project>.Filter.Eq(p => p.OwnerId, userId);
            var projects = _projects.Find(filter).ToList();
            return projects;
        }


        

        public Project UpdateProject(Project project)
        {
            try
            {
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



        public bool AddUserToProject(AddUserToProjectRequest request)
        {
            // Check if the Project exists
            var projectFilter = Builders<Project>.Filter.Eq(proj => proj.Id, request.ProjectId);
            var projectExists = _projects.Find(projectFilter).Any();
            if (!projectExists)
            {
                throw new Exception("Project couldn't be found");
            }
        
            // Check if the User exists and if the ProjectID is already part of the user's ProjectIDs.
            var userFilter = Builders<User>.Filter.Eq(user => user.Username, request.Username);
            var user = _user.Find(userFilter).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Username doesnt exist within the database");
            }

            if (user.ProjectID.Contains(request.ProjectId))
            {
                throw new Exception("This username is already within this project");
            }

            // Add the ProjectID to the user's list of ProjectIDs.
            var update = Builders<User>.Update.AddToSet(user => user.ProjectID, request.ProjectId);

            // Update the user in the database.
            var result = _user.UpdateOne(userFilter, update);

            // Check if the update was successful.
            return result.ModifiedCount > 0;
        }
    }
}
