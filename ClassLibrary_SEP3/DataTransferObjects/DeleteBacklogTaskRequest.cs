using System.ComponentModel.DataAnnotations;

namespace DefaultNamespace;

public class DeleteBacklogTaskRequest
{
    [Required]
    public int ProjectId { get; init; }
    [Required]
    public int TaskId { get; init; }
}