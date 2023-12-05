using System.ComponentModel.DataAnnotations;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class AddEntryPointRequest
{
    public String ProjectID { get; set; }
    
    public String OwnerUsername { get; set; }

    [Required] 
    public string Description { get; set; }

    [Required] 
    public DateTime CreatedTimeStamp { get; set;}
}
