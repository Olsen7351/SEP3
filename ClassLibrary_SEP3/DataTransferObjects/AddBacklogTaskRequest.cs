using ClassLibrary_SEP3;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using TaskStatus = ClassLibrary_SEP3.TaskStatus;

namespace ProjectMicroservice.DataTransferObjects;

public class AddBacklogTaskRequest
{
    [Required]
    public string ProjectId { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public TaskStatus Status { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime EstimateTime { get; set; }
    public DateTime ActualTimeUsed { get; set; }
    public string? Responsible { get; set; }
    //public List<TimeEntry>? TimeEntries { get; set; }
}