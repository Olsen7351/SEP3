using System;
using MongoDB.Bson;

namespace ProjectMicroservice.Models
{
    public class Backlog
    {
        public ObjectId Id { get; init; }
        public ObjectId ProjectId { get; init; }
        public string? Description { get; init; }
    }
}
