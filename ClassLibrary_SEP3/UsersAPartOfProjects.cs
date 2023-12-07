using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClassLibrary_SEP3;

public class UsersAPartOfProjects
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Username { get; set; }
    public List<String> ProjectID { get; set; }
    
    
    
}