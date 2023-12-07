using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public interface ILogBookService
{
    Task<IActionResult> CreateNewEntryLogBook(AddEntryPointRequest logBookEntryPoints);
    Task<LogBook> GetEntriesForLogBook(string projectID);
    Task<LogBookEntryPoints> GetSpecificEntry(string ProjectID, string projectID);
}