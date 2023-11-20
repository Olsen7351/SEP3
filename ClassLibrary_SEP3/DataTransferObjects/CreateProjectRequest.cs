using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservice.DataTransferObjects;

public class CreateProjectRequest
{
    [Required]
    [MaxLength(32)] 
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
}