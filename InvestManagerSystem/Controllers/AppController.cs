using Microsoft.AspNetCore.Mvc;

namespace InvestManagerSystem.Controllers
{
    [ApiController]
    [Route("/")]
    public class AppController : ControllerBase
    {
        // GET /
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("server is running");
        }
    }
}
