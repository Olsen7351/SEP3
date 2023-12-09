using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Broker.Services;

public interface IBBacklogService
{
    Task<IActionResult> CreateBacklogEntry(AddBacklogEntryRequest backlogEntry);
}