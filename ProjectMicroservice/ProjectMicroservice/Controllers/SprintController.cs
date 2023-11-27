using ClassLibrary_SEP3;
using ClassLibrary_SEP3.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Services;
namespace DefaultNamespace;

[Route("api/[controller]")]
[ApiController]
public class SprintController : ControllerBase
{
    private readonly ISprintService ISprintService;

    public SprintController(ISprintService sprintService)
    {
        this.ISprintService = sprintService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllSprintBacklogs(string ProjectId)
    {
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetSpecificSprintBacklog(string ProjectId, string id){
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSprintBacklog(string ProjectId,
        [FromBody] CreateSprintBackLogRequest request)
    {
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSprintBacklog(string ProjectId, string id,
        [FromBody] SprintBacklog sprintBacklog)
    {
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }
    
    [HttpDelete]
    
    public async Task<IActionResult> DeleteSprintBacklog(string ProjectId, string id)
    {
        return new StatusCodeResult(StatusCodes.Status501NotImplemented);
    }
}