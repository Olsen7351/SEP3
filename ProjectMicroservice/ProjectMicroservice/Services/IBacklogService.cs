﻿using ProjectMicroservice.Models;

namespace ProjectMicroservice.Services
{
    public interface IBacklogService
    {
        Backlog GetBacklogByProjectId(int projectId);
        Backlog CreateBacklog(int projectId, Backlog backlog);
        bool ProjectHasBacklog(int projectId);
    }
}