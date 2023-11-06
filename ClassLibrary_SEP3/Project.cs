using MongoDB.Bson;

namespace ProjectMicroservice.Models;

public class Project
{
    public ObjectId Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Backlog? Backlog { get; set; }
}