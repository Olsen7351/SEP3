using System.ComponentModel.DataAnnotations;
namespace ClassLibrary_SEP3;

public class LogBook
{
    [Required]
    public String ProjectID;

    public int LogBookID;
    
    public List<LogBookEntryPoints> LogBookEntryPoints { get; set; }
}