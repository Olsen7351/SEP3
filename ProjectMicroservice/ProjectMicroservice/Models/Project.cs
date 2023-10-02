namespace ProjectMicroservice.Models;

public class Project
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
}