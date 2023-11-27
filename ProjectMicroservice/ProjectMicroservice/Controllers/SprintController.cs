using Microsoft.AspNetCore.Mvc;
using ProjectMicroservice.Services;

namespace ProjectMicroservice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SprintController : ControllerBase
{
    public ISprintService SprintService;
    
    
}