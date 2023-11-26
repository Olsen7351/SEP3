using Microsoft.AspNetCore.Mvc;

namespace DefaultNamespace;

[Route("api/[controller]")]
[ApiController]
public class SprintController : ControllerBase
{
    public IProjectSprint ProjectSprint;
    
    
}