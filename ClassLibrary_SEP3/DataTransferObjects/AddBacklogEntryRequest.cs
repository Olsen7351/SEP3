using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3.DataTransferObjects;

public class AddBacklogEntryRequest
{
    public String ProjectID { get; set; }
    
    [Required(ErrorMessage = "RequirmentNr is required")]
    public String RequirmentNr { get; set; }
    public String EstimateTime { get; set; }
    public String ActualTime { get; set; }
    public String Status { get; set; }
    public String Sprint { get; set; }
}