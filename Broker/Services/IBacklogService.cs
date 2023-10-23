using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

namespace Broker.Services;

public interface IBacklogService
{
    Task<ActionResult> CreateAsync(Backlog backlog);
}