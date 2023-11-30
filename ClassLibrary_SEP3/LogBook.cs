using System.ComponentModel.DataAnnotations;
namespace ClassLibrary_SEP3;

public class LogBook
{
    [Required]
    private String ProjectID;
    
    private List<LogBookEntryPoints> LogBookEntryPoints { get; set; }
}