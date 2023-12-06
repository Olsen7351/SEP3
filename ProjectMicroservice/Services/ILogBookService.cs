using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;

namespace ProjectMicroservice.Services;

public interface ILogBookService
{
    LogBook CreateNewEntry(AddEntryPointRequest logBookEntryPoints);

    Task<LogBook> GetLogbookForProject(string projectID);
}