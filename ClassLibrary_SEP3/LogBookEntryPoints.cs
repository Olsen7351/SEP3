using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3;

public class LogBookEntryPoints
{
    public string ProjectID { get; set; }
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string EntryID { get; set; } = ObjectId.GenerateNewId().ToString();
    
    public string OwnerUsername { get; set; }

    [Required] 
    public string Description { get; set; }

    [Required] 
    public DateTime CreatedTimeStamp { get; set;}
}