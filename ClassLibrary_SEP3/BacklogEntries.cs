using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3;

public class BacklogEntries
{
    public String ProjectID { get; set; }
    public String RequirmentNr { get; set; }
    public String EstimateTime { get; set; }
    public String ActualTime { get; set; }
    public String Status { get; set; } // Changed to String type
    public String Sprint { get; set; }
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String BacklogEntryID { get; set; } = ObjectId.GenerateNewId().ToString();
}