namespace ClassLibrary_SEP3;

public class Requirement
{
    public int Id { get; init; }
    public int BacklogId { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public int Priority { get; set; }
}