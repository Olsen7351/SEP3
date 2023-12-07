using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class AddSprintTaskRequest
{
    [Required]
    public string ProjectId { get; set; }
    [Required]
    public string SprintId { get; set; }
    [Required]
    public string Title { get; set; }
    public string? Description { get; set; }
    [Required]
    public TaskStatus Status { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime Deadline { get; set; }
    public int EstimateTimeInMinutes { get; set; }
    public int ActualTimeUsedInMinutes { get; set; }
    public string? Responsible { get; set; }
}