using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface ITaskService
    {
        Models.TaskDatabase CreateTask(TaskDatabase task);
        Models.TaskDatabase GetTask(ObjectId id);
        bool DeleteTask(ObjectId id);
    }
}
