using ClassLibrary_SEP3;

namespace ProjectMicroservice.Services;

public interface ILogBookService
{
    LogBook CreateNewEntry(LogBookEntryPoints logBookEntryPoints);
    
    String GetLogbookForProject(String projectID);
}