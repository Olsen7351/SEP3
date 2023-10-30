using ClassLibrary_SEP3;

namespace ProjectMicroservice.Models;

public class Task
{
    public string TaskID { get; set; }
    public string BacklogID { get; set; }
    public string Description { get; set; }
    public string ProjectID { get; set; }
    public List<TimeEntry> TimeEntries { get; set; }
    
   
}