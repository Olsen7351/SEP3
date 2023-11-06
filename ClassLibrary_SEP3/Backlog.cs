using System;
using MongoDB.Bson;
using Task = ClassLibrary_SEP3.Task;

namespace ProjectMicroservice.Models
{
    public class Backlog
    {
        public List<Task>? BacklogTasks { get; init; }
    }
}
