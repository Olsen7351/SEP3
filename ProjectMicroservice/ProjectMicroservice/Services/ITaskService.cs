using MongoDB.Bson;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface ITaskService
    {
        Models.Task CreateTask(Models.Task task);
        Models.Task GetTask(ObjectId id);
    }
}
