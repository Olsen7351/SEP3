using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjectMicroservice.Models;

public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}