using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ProjectMicroservice.Models
{
    public class BacklogDatabase
    {
        [BsonId]
        public ObjectId Id { get; init; }
        public ObjectId ProjectId { get; init; }
        public string? Description { get; init; }
    }
}
