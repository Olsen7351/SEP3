﻿using MongoDB.Bson;
using ProjectMicroservice.DataTransferObjects;
using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface IBacklogService
    {
        BacklogDatabase GetBacklogByProjectId(ObjectId projectId);

        BacklogDatabase CreateBacklog(ObjectId projectId, CreateBacklogRequest backlog);
        bool ProjectHasBacklog(ObjectId projectId);
        bool BacklogBelongsToProject(ObjectId backlogId, ObjectId projectId);
    }
}
