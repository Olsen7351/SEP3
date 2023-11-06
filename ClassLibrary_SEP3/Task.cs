using MongoDB.Bson;

namespace ClassLibrary_SEP3
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done
    }
    public class Task
    {
        public ObjectId Id { get; init; }
        public ObjectId ProjectId { get; set; }
        public string Title { get; init; }
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EstimateTime { get; set; }
        public DateTime ActualTimeUsed { get; set; }
        public string Responsible { get; set; }
        public List<TimeEntry> TimeEntries { get; set; }
        
    }
}