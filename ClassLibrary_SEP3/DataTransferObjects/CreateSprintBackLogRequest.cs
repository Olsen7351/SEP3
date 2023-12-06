using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class CreateSprintBackLogRequest
{
    [Required]
    public string projectId { get; set; }  
    
    [Required]
    public string Title { get; set; }
    
    public DateTime Timestamp { get; set; }
}