using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services;

public interface ILogBookService
{
    Task<IActionResult> CreateNewEntryToLogBook(AddEntryPointRequest logBookEntryPoints);
    Task<LogBookEntryPoints> GetLogBookEntryByID(String EntryID);
    Task<IActionResult> UpdateEntry(String EntryID, String Description);
    Task<LogBook> GetEntriesForLogBook(string projectID);

}