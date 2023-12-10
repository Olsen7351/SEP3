using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Task = System.Threading.Tasks.Task;

namespace BlazorAppTEST.Services.Interface;

public interface IBacklogService
{
   Task<IActionResult> CreateBacklogEntry(AddBacklogEntryRequest backlogEntry);
   Task<BBackLog> GetBacklogForProject(string projectIdAsString);
}