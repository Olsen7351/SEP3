using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3;

public class LogBookEntryPoints
{
    public int LogBookID { get; set; }
    [Required] 
    public String OwnerUsername { get; set; }

    [Required] 
    public string Description { get; set; }

    [Required] 
    public DateTime CreatedTimeStamp { get; set;} = DateTime.Today;
}