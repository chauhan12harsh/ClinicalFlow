using Microsoft.AspNetCore.Mvc;

namespace ClinicalFlow.Controllers
{
    [ApiController]
    [Route("/api/")]
    public class HealthController: ControllerBase
    {

        [HttpGet("health")]
        public IActionResult GetHealth() { 
        
            return Ok( new 
                { 
                    Status = "Healthy",
                    Application = "Clinical Management Api",
                    TimeStamp = DateTime.UtcNow
                }
            );
                
        }
    }
}
