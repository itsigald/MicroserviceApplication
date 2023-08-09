using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;

namespace CommandsService.Controllers
{
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ILogger<CommandsController> _logger;
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo, IMapper mapper, ILogger<CommandsController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CommandDto>>> GetCommandForPlatform(int platformId)
        {
            _logger.LogInformation($"Called GetCommandForPlatform with platformId {platformId}");

            if(!(await _repo.PlatformExists(platformId)))
                return NotFound();

            var commandItems = _repo.GetCommandForPlatform(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandDto>>(commandItems));
        }

        [HttpGet("{commandId}", Name ="GetCommandForPlatform")]
        public async Task<ActionResult<CommandDto>> GetCommandForPlatform(int platformId, int commandId)
        {
            _logger.LogInformation($"Called GetCommandForPlatform with platformId {platformId} and commandId {commandId}");

            if (!(await _repo.PlatformExists(platformId)))
                return NotFound();

            var command = _repo.GetCommand(platformId, commandId);

            if(command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandDto>(command));
        }

        [HttpPost]
        public async Task<ActionResult<CommandDto>> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            _logger.LogInformation($"Called CreateCommandForPlatform with platformId {platformId}");

            if (!(await _repo.PlatformExists(platformId)))
                return NotFound();

            var command = _mapper.Map<Command>(commandDto);
            _repo.CreateCommand(platformId, command);
            await _repo.SaveChangesAsync();

            var commandCreated = _mapper.Map<CommandDto>(command);

            return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId = platformId, commandId = commandCreated.Id }, commandCreated);
        }
    }
}
