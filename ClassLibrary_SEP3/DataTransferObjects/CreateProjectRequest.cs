using System.ComponentModel.DataAnnotations;
using BlazorAppTEST.Services;

namespace ProjectMicroservice.DataTransferObjects;


[ValidEndDate("StartDate", "EndDate")]
public class CreateProjectRequest
{
    [Required(ErrorMessage = "Project name is required")]
    [MaxLength(32, ErrorMessage = "Name cannot exceed 32 characters")] 
    public string? Name { get; set; }

    [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }
    
    [Required]
    [CheckDateHelper(ErrorMessage = "Start date must be in the present or future")]
    public DateTime StartDate { get; set; }

    
    [Required]
    public DateTime EndDate { get; set; }
}