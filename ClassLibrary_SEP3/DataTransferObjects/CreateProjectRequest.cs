using System.ComponentModel.DataAnnotations;
using BlazorAppTEST.Services;

namespace ProjectMicroservice.DataTransferObjects;


[ValidEndDate("StartDate", "EndDate")]
public class CreateProjectRequest
{
    [Required(ErrorMessage = "Project name is required")]
    [MaxLength(32)] 
    public string? Name { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    [CheckDateHelper(ErrorMessage = "Start date must be in the present or future")]
    public DateTime StartDate { get; set; }

    
    [Required]
    public DateTime EndDate { get; set; }
}