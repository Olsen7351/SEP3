using ClassLibrary_SEP3;
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
    public class TaskDatabase
    {
        [BsonId]
        public ObjectId Id { get; init; }
        public ObjectId ProjectId { get; init; }
        public required string Title { get; init; }
        public string? Description { get; init; }
        public ClassLibrary_SEP3.TaskStatus Status { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? EstimateTime { get; init; }
        public DateTime? ActualTimeUsed { get; init; }
        public string? Responsible { get; init; }
        //public List<TimeEntry>? TimeEntries { get; init; }

    }
}