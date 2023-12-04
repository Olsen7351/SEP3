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


    
    
    //Get
    public string GetLogbookForProject(string projectID)
    {
        throw new NotImplementedException();
    }
}