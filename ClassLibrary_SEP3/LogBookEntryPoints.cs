using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3;

public class LogBookEntryPoints
{
    public String ProjectID { get; set; }
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string EntryID { get; set; }
    
    public String OwnerUsername { get; set; }

    [Required] 
    public string Description { get; set; }

    [Required] 
    public DateTime CreatedTimeStamp { get; set;}
}