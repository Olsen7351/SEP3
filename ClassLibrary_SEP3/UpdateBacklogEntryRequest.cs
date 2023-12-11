namespace ClassLibrary_SEP3;

public class UpdateBacklogEntryRequest
{
    public string EntryID { get; set; }
    public string ProjectID { get; set; }
    
    
    //Stuff to edit
    public String RequirmentNr { get; set; }
    public String EstimateTime { get; set; }
    public String ActualTime { get; set; }
    public String Status { get; set; } 
    public String Sprint { get; set; }
}