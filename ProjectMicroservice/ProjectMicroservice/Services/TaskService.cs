using MongoDB.Bson;
using MongoDB.Driver;
using ProjectMicroservice.Data;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public class TaskService : ITaskService
    {
        private readonly IMongoCollection<Models.TaskDatabase> _tasks;

        public TaskService(MongoDbContext context)
        {
            _tasks = context.Database.GetCollection<Models.TaskDatabase>("Tasks");
        }

        public Models.TaskDatabase CreateTask(Models.TaskDatabase task)
        {
            _tasks.InsertOne(task);
            return task;
        }

        public Models.TaskDatabase GetTask(ObjectId id)
        {
            return _tasks.Find(t => t.Id == id).FirstOrDefault();
        }

        public bool DeleteTask(ObjectId id)
        {
            var result = _tasks.DeleteOne(t => t.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
