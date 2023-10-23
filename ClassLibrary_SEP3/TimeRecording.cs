namespace ProjectMicroservice.Models;

public class TimeRecording
{
    public int Id { get; init; }
    public int ProjectId { get; init; }
    public string Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Notes { get; set; } //to add additional notes or descriptions related to the time entry.
}