using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace ProjectMicroservice.Services;

public interface IBacklogService
{
    Task<IActionResult> CreateBacklogEntry(AddBacklogEntryRequest backlogEntry);
}