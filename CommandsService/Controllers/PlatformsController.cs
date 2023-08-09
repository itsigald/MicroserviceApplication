using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformsController> _logger;

        public PlatformsController(ICommandRepo repo, IMapper mapper, ILogger<PlatformsController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommandDto>>> GetPlatoforms()
        {
            _logger.LogInformation("--> Getting Platforms from CommandService");
            var platformItems = await _repo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformDto>>(platformItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            _logger.LogInformation("---> Inbound POST from Commands Service");
            return Ok("Inbound test OK from Commands Service");
        }
    }
}
