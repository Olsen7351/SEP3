using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ClassLibrary_SEP3;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Backlog? Backlog { get; set; }
    public string LogString()
    {
        return $"Project Id: {Id}, Name: {Name}, Description: {Description}, StartDate: {StartDate}, EndDate: {EndDate}";
    }

}