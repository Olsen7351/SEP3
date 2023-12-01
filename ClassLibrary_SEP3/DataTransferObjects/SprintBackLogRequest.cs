namespace ClassLibrary_SEP3.DataTransferObjects;

public class SprintBackLogRequest
{
    public string SprintBacklog_ProjectID { get; set; }
    public string SprintBacklog_Id { get; set; }
    public string SprintBacklog_Title { get; set; }
    
    public List<Task> SprintBacklog_Tasks { get; set; }
}