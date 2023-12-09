using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using ProjectMicroservice.Data;

namespace ProjectMicroservice.Services;

public class LogBookService : ILogBookService
{
    private readonly IMongoCollection<LogBook> _logEntryPoints;


    public LogBookService(MongoDbContext context)
    {
        _logEntryPoints = context.Database.GetCollection<LogBook>("LogBook");
    }


    public LogBook CreateNewEntry(AddEntryPointRequest logBookEntryPoints)
    {
        // Check if the CreatedTimeStamp is set to the current date
        if (logBookEntryPoints.CreatedTimeStamp.Date != DateTime.Today)
        {
            throw new Exception("Created entry needs to have a present timestamp");
        }

        if (String.IsNullOrEmpty(logBookEntryPoints.ProjectID))
        {
            throw new Exception("ProjectID cant be null or empty");
        }

        if (string.IsNullOrEmpty(logBookEntryPoints.OwnerUsername))
        {
            throw new Exception("Owner username cant be null or empty");
        }

        var filter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, logBookEntryPoints.ProjectID);
        var logBook = _logEntryPoints.Find(filter).FirstOrDefault();
        var newEntry = new LogBookEntryPoints()
        {
            ProjectID = logBookEntryPoints.ProjectID,
            OwnerUsername = logBookEntryPoints.OwnerUsername,
            Description = logBookEntryPoints.Description,
            CreatedTimeStamp = logBookEntryPoints.CreatedTimeStamp
        };

        if (logBook == null)
        {
            logBook = new LogBook
            {
                ProjectID = logBookEntryPoints.ProjectID,
                LogBookEntryPoints = new List<LogBookEntryPoints> { newEntry } 
            };
            _logEntryPoints.InsertOne(logBook);
        }
        else
        {
            var update = Builders<LogBook>.Update.Push(lb => lb.LogBookEntryPoints, newEntry);
            _logEntryPoints.UpdateOne(filter, update);
        }

        return logBook;
    }



    public async Task<LogBook> GetLogbookForProject(string projectID)
    {
        // Create a filter to find the LogBook document with the matching ProjectID
        var filter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, projectID);
        var logBook = await _logEntryPoints.Find(filter).FirstOrDefaultAsync();

        // If no LogBook is found, throw an exception
        if (logBook == null)
        {
            throw new KeyNotFoundException("No logbook found for the given project ID.");
        }
        // If a LogBook is found, return it
        return logBook;
    }

    
    
    
    public async Task<LogBookEntryPoints> GetSpecificLogBookEntry(string projectId, string entryId)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            throw new ArgumentException("ProjectID is required.");
        }

        if (string.IsNullOrEmpty(entryId))
        {
            throw new ArgumentException("EntryID is required.");
        }

        // Convert the entryId to an ObjectId
        if (!ObjectId.TryParse(entryId, out var objectId))
        {
            throw new ArgumentException("EntryID is not a valid ObjectId.");
        }

        // First, filter to find the LogBook with the specified ProjectID
        var logBookFilter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, projectId);
        var logBook = await _logEntryPoints.Find(logBookFilter).FirstOrDefaultAsync();

        // If no LogBook is found
        if (logBook == null)
        {
            throw new KeyNotFoundException("No logbook found for the given project ID.");
        }

        // Then, find the specific LogBookEntryPoints within the LogBook's EntryPoints
        var logBookEntry = logBook.LogBookEntryPoints
            .FirstOrDefault(entry => entry.EntryID == objectId.ToString());

        // If no LogBookEntryPoints is found
        if (logBookEntry == null)
        {
            throw new KeyNotFoundException("No logbook entry found for the given entry ID.");
        }

        return logBookEntry;
    }

    
    
    public async Task<bool> UpdateLogBookEntry(UpdateEntryRequest updateEntryRequest)
    {
        if (string.IsNullOrEmpty(updateEntryRequest.ProjectID))
        {
            throw new ArgumentException("ProjectID is required.");
        }

        if (string.IsNullOrEmpty(updateEntryRequest.EntryID))
        {
            throw new ArgumentException("EntryID is required.");
        }

        // Convert the entryId to an ObjectId
        if (!ObjectId.TryParse(updateEntryRequest.EntryID, out var objectId))
        {
            throw new ArgumentException("EntryID is not a valid ObjectId.");
        }

        // Create a filter to find the LogBook document with the matching ProjectID
        var logBookFilter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, updateEntryRequest.ProjectID);

        // Find the LogBook
        var logBook = await _logEntryPoints.Find(logBookFilter).FirstOrDefaultAsync();
        if (logBook == null)
        {
            throw new KeyNotFoundException("No logbook found for the given project ID.");
        }

        // Find the index of the entry to update
        var entryIndex = logBook.LogBookEntryPoints.FindIndex(entry => entry.EntryID == objectId.ToString());
        if (entryIndex == -1)
        {
            throw new KeyNotFoundException("No logbook entry found for the given entry ID.");
        }

        // Define the update
        var updateDefinition = Builders<LogBook>.Update
                .Set(lb => lb.LogBookEntryPoints[entryIndex].Description, updateEntryRequest.Description)
            // Add other fields to update as needed
            ;

        // Perform the update
        var updateResult = await _logEntryPoints.UpdateOneAsync(logBookFilter, updateDefinition);

        // Check if the update was successful
        return updateResult.ModifiedCount == 1;
    }
}