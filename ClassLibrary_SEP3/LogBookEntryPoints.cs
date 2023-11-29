using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3;

public class LogBookEntryPoints
{
    [Required] 
    public String OwnerUsername { get; set; }

    [Required] 
    public string Description { get; set; }

    [Required] 
    public DateTime createdTimeStamp { get; set; }
}