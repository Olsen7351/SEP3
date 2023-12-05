using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
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
        // Check if the CreatedTimeStamp is set to the current date (you might not need this if it's set elsewhere)
        if (logBookEntryPoints.CreatedTimeStamp.Date != DateTime.Today)
        {
            throw new Exception("Created entry needs to have a present timestamp");
        }
    
        // Find the LogBook with the given ProjectID or create a new one
        var filter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, logBookEntryPoints.ProjectID);
        var logBook = _logEntryPoints.Find(filter).FirstOrDefault();

        if (logBook == null)
        {
            logBook = new LogBook
            {
                ProjectID = logBookEntryPoints.ProjectID, // This line was corrected
                LogBookEntryPoints = new List<LogBookEntryPoints> { logBookEntryPoints }
            };
            _logEntryPoints.InsertOne(logBook);
        }
        
        else
        {
            var newEntry = new LogBookEntryPoints()
            {
                ProjectID = logBookEntryPoints.ProjectID,
                OwnerUsername = logBookEntryPoints.OwnerUsername,
                Description = logBookEntryPoints.Description,
                CreatedTimeStamp = logBookEntryPoints.CreatedTimeStamp
            };
    
            var update = Builders<LogBook>.Update.Push(lb => lb.LogBookEntryPoints, newEntry);
            _logEntryPoints.UpdateOne(filter, update);
        }
        return logBook;
    }



    public async Task<LogBook> GetLogbookForProject(string projectID)
    {
        // Create a filter to find the LogBook document with the matching ProjectID
        var filter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, projectID);
    
        // Perform an asynchronous query to find the first LogBook that matches the filter
        var logBook = await _logEntryPoints.Find(filter).FirstOrDefaultAsync();
    
        // If no LogBook is found, throw an exception
        if (logBook == null)
        {
            throw new KeyNotFoundException("No logbook found for the given project ID.");
        }
        // If a LogBook is found, return it
        return logBook;
    }
}