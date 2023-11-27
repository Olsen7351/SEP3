
using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using DefaultNamespace;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Services;

namespace ProjectMicroservice.Controllers;



[Route("api/[controller]")]
[ApiController]
public class SprintController : ControllerBase
{

    public ISprintService SprintService;

    private readonly ISprintService _sprintService;

    public SprintController(ISprintService sprintService)
    {
        this._sprintService = sprintService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllSprintBacklogs(string ProjectId)
    {
        try
        {
            var sprintBacklogs = _sprintService.GetAllSprintBacklogs(ProjectId);
            return Ok(sprintBacklogs);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecificSprintBacklog(string ProjectId, string id)
    {
        try
        {
            var sprintBacklog = _sprintService.GetSprintBacklogById(id);
            if (sprintBacklog == null)
            {
                return NotFound();
            }

            return Ok(sprintBacklog);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateSprintBacklog(string ProjectId, [FromBody] CreateSprintBackLogRequest request)
    {
        try
        {
            var newSprintBacklog = _sprintService.CreateSprintBacklog(request);
            return CreatedAtAction(nameof(GetSpecificSprintBacklog), new { ProjectId, id = newSprintBacklog.ProjectId }, newSprintBacklog);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSprintBacklog(string ProjectId, string id, [FromBody] SprintBacklog sprintBacklog)
    {
        try
        {
            var updatedSprintBacklog = _sprintService.UpdateSprintBacklog(id, sprintBacklog);
            if (updatedSprintBacklog == null)
            {
                return NotFound();
            }

            return Ok(updatedSprintBacklog);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSprintBacklog(string ProjectId, string id)
    {
        try
        {
            var isDeleted = _sprintService.DeleteSprintBacklog(id);
            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();

        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        }
    }
}