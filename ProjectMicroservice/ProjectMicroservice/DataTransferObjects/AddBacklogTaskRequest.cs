using System.ComponentModel.DataAnnotations;
using System.Dynamic;

namespace ProjectMicroservice.DataTransferObjects;

public class AddBacklogTaskRequest
{
    [Required]
    public int ProjectId { get; set; }
    [Required]
    public int TaskId { get; set; }
    [Required]
    
    [MaxLength(100)]
    public string Title { get; set; }
}