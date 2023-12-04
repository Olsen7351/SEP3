using ClassLibrary_SEP3;

namespace ProjectMicroservice.Services;

public interface ILogBookService
{
    LogBook CreateNewEntry(LogBookEntryPoints logBookEntryPoints);

    Task<LogBook> GetLogbookForProject(string projectID);
}