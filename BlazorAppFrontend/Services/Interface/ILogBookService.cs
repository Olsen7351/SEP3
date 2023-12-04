using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAppTEST.Services;

public interface ILogBookService
{
    Task<IActionResult> CreateNewEntryToLogBook(LogBookEntryPoints logBookEntryPoints);
}