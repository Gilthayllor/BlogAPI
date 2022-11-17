using BlogAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromServices] IConfiguration configuration)
        {
            var env = configuration.GetValue<string>("Env");
            return Ok(new { 
                environment = env
            });
        }
    }
}