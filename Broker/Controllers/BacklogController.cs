﻿using Broker.Services;
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;

namespace Broker.Controllers;

[Route("api/[controller]")]
public class BacklogController : ControllerBase
{
    private readonly IBacklogService _backlogService;

    public BacklogController(IBacklogService backlogService)
    {
        _backlogService = backlogService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBacklog([FromBody] int ProjectId)
    {
        if (ProjectId < 0)
        {
            return BadRequest();
        }
        
        return Ok(await _backlogService.GetBacklog(ProjectId));
    }
    [HttpPost]
    public async Task<IActionResult> CreateBacklog([FromBody] Backlog backlog)
    {
        if (backlog == null)
        {
            return BadRequest();
        }
        
        return Ok(await _backlogService.CreateBacklog(backlog));
    }
    
    [HttpPost("{ProjectId}/Backlog/{IdBacklog}")]
    public async Task<IActionResult> AddTaskToBacklog([FromRoute] int ProjectId, [FromRoute] int IdBacklog, [FromBody] BackLogTask backLogTask)
    {
        if (backLogTask == null)
        {
            return BadRequest();
        }
        return Ok(await _backlogService.AddTaskToBackLog(ProjectId,IdBacklog,backLogTask));
    }
}