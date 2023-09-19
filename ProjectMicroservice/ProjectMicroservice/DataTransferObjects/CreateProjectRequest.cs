using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservice.DataTransferObjects;

public class CreateProjectRequest
{
    [Required]
    [MaxLength(32)] // TODO: Change to reflect actual DB constraint
    public string? Name { get; init; }

    [MaxLength(1000)] // TODO: Change to reflect actual DB constraint
    public string? Description { get; init; }

    [Required]
    public DateTime StartDate { get; init; }

    [Required]
    public DateTime EndDate { get; init; }
}