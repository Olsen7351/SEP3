using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservice.DataTransferObjects;

public class CreateProjectRequest
{
    [Required]
    [MaxLength(32)] // TODO: Change to reflect actual DB constraint
    public string? Name { get; set; }

    [MaxLength(1000)] // TODO: Change to reflect actual DB constraint
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}