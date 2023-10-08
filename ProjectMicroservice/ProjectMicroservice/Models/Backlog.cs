using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ProjectMicroservice.Models
{
    public class Backlog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; init; }
        public int ProjectId { get; init; }
        public string? Description { get; init; }
    }
}
