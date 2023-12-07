using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services;

public interface ILogBookService
{
    Task<IActionResult> CreateNewEntryToLogBook(AddEntryPointRequest logBookEntryPoints);
    Task<LogBookEntryPoints> GetSpecificLogBookEntryByID(String EntryID, String ProjectID);
    Task<IActionResult> UpdateEntry(UpdateEntryRequest updateEntryRequest);
    Task<LogBook> GetEntriesForLogBook(string projectID);

}