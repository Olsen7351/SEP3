namespace ClassLibrary_SEP3.DataTransferObjects;

public class AddUserToProjectRequest
{
    public string Username{ get; set; }
    public string ProjectId { get; set; }

    public string LogString()
    {
        return $"Username: {Username}, ProjectId: {ProjectId}";
    }
}