using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class CreateSprintBackLogRequest
{
    [Required]
    public string ProjectId { get; set; }
    [Required]
    public string Id { get; set; }
    [Required]
    public string Title { get; set; }
    
    public DateTime Timestamp { get; set; }
    public List<Task> Tasks { get; set; }
}