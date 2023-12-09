using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3;

public class BBackLog
{
    public String ProjectID { get; set; }
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public String BBacklogID { get; set; }
    public List<BacklogEntries> BacklogEntriesList { get; set; }
}