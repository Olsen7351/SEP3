using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace ProjectMicroservice.DataTransferObjects;

public class AddBacklogTaskRequest
{
    [Required]
    public ObjectId ProjectId { get; init; }
    [Required]
    public ObjectId BacklogId { get; init; } // Associating with a specific backlog
    public string Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime EstimateTime { get; set; }
    public DateTime ActualTimeUsed { get; set; }
    public string Responsible { get; set; }
}