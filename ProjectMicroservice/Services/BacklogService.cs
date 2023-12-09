﻿using ClassLibrary_SEP3;
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
}