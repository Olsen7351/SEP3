using ClassLibrary_SEP3;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ClassLibrary_SEP3.DataTransferObjects;
using ProjectMicroservice.Data;

namespace ProjectMicroservice.Services;

public class BacklogService : IBacklogService
{
    private readonly IMongoCollection<BBackLog> _BBackLogCollection;

    
    public BacklogService(MongoDbContext context)
    {
        _BBackLogCollection = context.Database.GetCollection<BBackLog>("BackLog");
    }

    
    public async Task<IActionResult> CreateBacklogEntry(AddBacklogEntryRequest addRequest)
    {
        if (_BBackLogCollection == null)
        {
            throw new Exception("Database is not connected");
        }
        
        if (addRequest == null)
        {
            return new BadRequestObjectResult("Backlog entry request is null");
        }

        if (String.IsNullOrEmpty(addRequest.ProjectID))
        {
            return new BadRequestObjectResult("ProjectID cannot be null or empty");
        }

        var filter = Builders<BBackLog>.Filter.Eq(bb => bb.ProjectID, addRequest.ProjectID);
        var bBackLog = await _BBackLogCollection.Find(filter).FirstOrDefaultAsync();

        var newBacklogEntry = new BacklogEntries()
        {
            ProjectID = addRequest.ProjectID,
            RequirmentNr = addRequest.RequirmentNr,
            EstimateTime = addRequest.EstimateTime,
            ActualTime = addRequest.ActualTime,
            Status = addRequest.Status,
            Sprint = addRequest.Sprint
        };
    
        if (bBackLog == null)
        {
            bBackLog = new BBackLog
            {
                ProjectID = addRequest.ProjectID,
                BacklogEntriesList = new List<BacklogEntries> { newBacklogEntry }
            };
            await _BBackLogCollection.InsertOneAsync(bBackLog);
        }
        else
        {
            // Add the new backlogEntry to the existing BBackLog
            var update = Builders<BBackLog>.Update.Push(bb => bb.BacklogEntriesList, newBacklogEntry);
            await _BBackLogCollection.UpdateOneAsync(filter, update);
        }
        return new OkObjectResult(bBackLog);
    }

    
    public async Task<BBackLog> GetBacklogForProject(string projectId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new ArgumentException("ProjectID can't be null or empty", nameof(projectId));
        }

        if (_BBackLogCollection == null)
        {
            throw new InvalidOperationException("Backlog collection is not initialized.");
        }

        var filter = Builders<BBackLog>.Filter.Eq(bb => bb.ProjectID, projectId);
        var backlog = await _BBackLogCollection.Find(filter).FirstOrDefaultAsync();

        if (backlog == null)
        {
            throw new KeyNotFoundException($"Backlog for project ID '{projectId}' not found.");
        }

        return backlog;
    }

    public async Task<BacklogEntries> GetSpecificBacklogEntry(string projectId, string backlogEntryId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new ArgumentException("ProjectID can't be null or empty", nameof(projectId));
        }

        if (string.IsNullOrEmpty(backlogEntryId))
        {
            throw new ArgumentException("BacklogEntryID can't be null or empty", nameof(backlogEntryId));
        }

        if (_BBackLogCollection == null)
        {
            throw new InvalidOperationException("Backlog collection is not initialized.");
        }

        // Find the backlog for the specified project
        var filter = Builders<BBackLog>.Filter.Eq(bb => bb.ProjectID, projectId);
        var backlog = await _BBackLogCollection.Find(filter).FirstOrDefaultAsync();
        if (backlog == null)
        {
            throw new KeyNotFoundException($"Backlog for project ID '{projectId}' not found.");
        }

        // Find the specific backlog entry within the found backlog
        var backlogEntry = backlog.BacklogEntriesList?.FirstOrDefault(be => be.BacklogEntryID == backlogEntryId);
        if (backlogEntry == null)
        {
            throw new KeyNotFoundException($"Backlog entry with ID '{backlogEntryId}' for project ID '{projectId}' not found.");
        }

        return backlogEntry;
    }

    public async Task<bool> UpdateBacklogEntry(UpdateBacklogEntryRequest updateBacklogEntryRequest)
    {
        if (_BBackLogCollection == null)
        {
            throw new InvalidOperationException("Backlog collection is not initialized.");
        }

        if (string.IsNullOrEmpty(updateBacklogEntryRequest.ProjectID) || string.IsNullOrEmpty(updateBacklogEntryRequest.EntryID))
        {
            throw new ArgumentException("ProjectID and EntryID can't be null or empty");
        }

        // Find the specific backlog
        var filter = Builders<BBackLog>.Filter.Eq(bb => bb.ProjectID, updateBacklogEntryRequest.ProjectID);
        var backlog = await _BBackLogCollection.Find(filter).FirstOrDefaultAsync();
        if (backlog == null || backlog.BacklogEntriesList == null)
        {
            return false; // Backlog or entry not found
        }

        // Find and update the specific backlog entry
        var entryToUpdate = backlog.BacklogEntriesList.FirstOrDefault(be => be.BacklogEntryID == updateBacklogEntryRequest.EntryID);
        if (entryToUpdate != null)
        {
            // Update fields
            entryToUpdate.RequirmentNr = updateBacklogEntryRequest.RequirmentNr;
            entryToUpdate.EstimateTime = updateBacklogEntryRequest.EstimateTime;
            entryToUpdate.ActualTime = updateBacklogEntryRequest.ActualTime;
            entryToUpdate.Status = updateBacklogEntryRequest.Status;
            entryToUpdate.Sprint = updateBacklogEntryRequest.Sprint;

            // Update the document in MongoDB
            var update = Builders<BBackLog>.Update.Set(bb => bb.BacklogEntriesList, backlog.BacklogEntriesList);
            var updateResult = await _BBackLogCollection.UpdateOneAsync(filter, update);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
        else
        {
            return false; // Specific entry not found
        }
    }
}