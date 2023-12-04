using ClassLibrary_SEP3;
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


    // Create 
    public LogBook CreateNewEntry(LogBookEntryPoints logBookEntryPoints)
    {
        // Find the LogBook with the given LogBookID or create a new one
        var filter = Builders<LogBook>.Filter.Eq(lb => lb.LogBookID, logBookEntryPoints.LogBookID);
        var logBook = _logEntryPoints.Find(filter).FirstOrDefault();

        if (logBook == null)
        {
            logBook = new LogBook
            {
                LogBookID = logBookEntryPoints.LogBookID,
                LogBookEntryPoints = new List<LogBookEntryPoints> { logBookEntryPoints }
            };
            _logEntryPoints.InsertOne(logBook);
        }
        else
        {
            var update = Builders<LogBook>.Update.Push(lb => lb.LogBookEntryPoints, logBookEntryPoints);
            _logEntryPoints.UpdateOne(filter, update);
        }

        return logBook;
    }


    public async Task<LogBook> GetLogbookForProject(string projectID)
    {
        // Assuming your LogBook documents have a "ProjectID" field to link to the Project
        var filter = Builders<LogBook>.Filter.Eq(lb => lb.ProjectID, projectID);
        var logBook = await _logEntryPoints.Find(filter).FirstOrDefaultAsync();
        if (logBook == null)
        {
            // Handle the case where there is no logbook for the given projectID, if necessary
            throw new KeyNotFoundException("No logbook found for the given project ID.");
        }

        return logBook;
    }
}