using System.ComponentModel.DataAnnotations;
using ClassLibrary_SEP3.Helper;

namespace ClassLibrary_SEP3.DataTransferObjects;


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

    [Required]
    public string ByUsername { get; set; }

    public string LogString()
    {
        return "Name: " + Name + ", Description: " + Description + ", StartDate: " + StartDate + ", EndDate: " + EndDate + ", ByUsername: " + ByUsername;
    }
}