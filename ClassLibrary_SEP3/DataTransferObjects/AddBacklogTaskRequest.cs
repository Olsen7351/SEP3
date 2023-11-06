using ClassLibrary_SEP3;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;

namespace ProjectMicroservice.DataTransferObjects;

public class AddBacklogTaskRequest
{
    [Required]
    public ObjectId ProjectId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EstimateTime { get; set; }
    public DateTime ActualTimeUsed { get; set; }
    public string? Responsible { get; set; }
    public List<TimeEntry>? TimeEntries { get; set; }
}