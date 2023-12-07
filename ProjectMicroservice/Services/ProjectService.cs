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
        private readonly IMongoCollection<UsersAPartOfProjects> _users;


        public ProjectService(MongoDbContext context)
        {
            _projects = context.Database.GetCollection<Project>("Projects");
            _users = context.Database.GetCollection<UsersAPartOfProjects>("ProjectsForUsers");
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
            var filter = Builders<UsersAPartOfProjects>.Filter.Eq(u => u.Username, request.Username);

            // Check if the user already exists
            var existingUser = _users.Find(filter).FirstOrDefault();

            if (existingUser != null)
            {
                // User exists, so update the user's projects using AddToSet to avoid duplicates
                var update = Builders<UsersAPartOfProjects>.Update.AddToSet(list => list.ProjectID, request.ProjectId);
                var result = _users.UpdateOne(filter, update);
                return result.ModifiedCount > 0;
            }
            else
            {
                // User does not exist, so create a new user and add the project ID
                var newUser = new UsersAPartOfProjects
                {
                    Username = request.Username,
                    ProjectID = new List<string> { request.ProjectId }
                    // Add other necessary fields here
                };

                _users.InsertOne(newUser);
                return true; // Since the user is newly added, we return true
            }
        }
    }
}
    

