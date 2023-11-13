using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3
{
    public class Task
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ProjectId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EstimateTime { get; set; }
        public DateTime ActualTimeUsed { get; set; }
        public string Responsible { get; set; }
        //public List<TimeEntry> TimeEntries { get; set; }
        
    }
}