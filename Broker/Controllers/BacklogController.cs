﻿using Broker.Services;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Models;
using Task = ClassLibrary_SEP3.Task;

namespace Broker.Controllers;

[Route("api/[controller]")]
public class BacklogController : ControllerBase
{
    private readonly IBacklogService _backlogService;

    public BacklogController(IBacklogService backlogService)
    {
        _backlogService = backlogService;
    }
    
    //GetByID
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
    
    
    //AddTaskToBacklog
    [HttpPost("{ProjectId}/Backlog/{IdBacklog}")]
    public async Task<IActionResult> AddTaskToBacklog([FromRoute] int ProjectId, [FromRoute] int IdBacklog, [FromBody] Task backLogTask)
    {
        if (backLogTask == null)
        {
            return BadRequest();
        }
        return Ok(await _backlogService.AddTaskToBackLog(ProjectId,backLogTask));
    }

    [HttpDelete("{ProjectId}/Backlog/{IdBacklog}")]
    public async Task<IActionResult> DeleteTaskFromBacklog([FromRoute] int ProjectId, [FromRoute] int IdBacklog,
        [FromBody] DeleteBacklogTaskRequest backlogTask)
    {
        if (backlogTask == null)
        {
            return BadRequest();
        }

        return Ok(await _backlogService.DeleteTaskFromBacklog(ProjectId, IdBacklog,backlogTask));
    }
}