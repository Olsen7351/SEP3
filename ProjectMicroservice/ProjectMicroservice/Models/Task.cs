using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ProjectMicroservice.Models
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }
    public class Task
    {
        [BsonId]
        public ObjectId Id { get; init; }
        public ObjectId ProjectId { get; init; }
        public ObjectId BacklogId { get; init; } // Associating with a specific backlog
        public string Title { get; init; }
        public string? Description { get; init; }
        public TaskStatus Status { get; init; }
        public DateTime CreatedAt { get; set; }
    }
}