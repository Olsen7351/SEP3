using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace ProjectMicroservice.Services;

public interface IBacklogService
{
    Task<IActionResult> CreateBacklogEntry(AddBacklogEntryRequest backlogEntry);
    Task<BBackLog> GetBacklogForProject(string projectId);
    Task<BacklogEntries> GetSpecificBacklogEntry(string projectId, string backlogEntryId);
    Task<bool> UpdateBacklogEntry(UpdateBacklogEntryRequest updateBacklogEntryRequest);
}