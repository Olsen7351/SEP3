using MongoDB.Bson;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMongoCollection<Models.Task> _tasks;

        public TaskService(MongoDbContext context)
        {
            _tasks = context.Database.GetCollection<Models.Task>("Tasks");
        }

        public Models.Task CreateTask(Models.Task task)
        {
            // Set creation time
            task.CreatedAt = System.DateTime.UtcNow;
            _tasks.InsertOne(task);
            return task;
        }

        public Models.Task GetTask(ObjectId id)
        {
            return _tasks.Find(t => t.Id == id).FirstOrDefault();
        }
    }
}
