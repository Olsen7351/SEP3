using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services;

public interface ILogBookService
{
    Task<IActionResult> CreateNewEntryToLogBook(LogBookEntryPoints logBookEntryPoints);
    Task<IActionResult> GetLogBookEntryByID(String EntryID);
    Task<IActionResult> UpdateEntry(String EntryID, String Description);
    Task<IActionResult> GetEntriesForLogBook(String projectID);
}