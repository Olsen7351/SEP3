using MongoDB.Bson;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface ITaskService
    {
        Models.TaskDatabase CreateTask(Models.TaskDatabase task);
        Models.TaskDatabase GetTask(ObjectId id);
        bool DeleteTask(ObjectId id);
    }
}
