using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ILogger<PlatformsController> _logger;
        public PlatformsController(ILogger<PlatformsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            _logger.LogInformation("---> Inbound POST from Commands Service");
            return Ok("Inbound test OK from Commands Service");
        }
    }
}
