namespace DefaultNamespace;

public class BackLogTask
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool isCompleted { get; set; } = false;
    public string Responsible { get; set; }
}