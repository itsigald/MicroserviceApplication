using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using System.ComponentModel.DataAnnotations;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;

        public PlatformController(IPlatformRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformDto>>> GetPlatforms()
        {
            var platforms = await _repo.GetAllPlatformsAsync();

            if(platforms == null)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<PlatformDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<ActionResult<PlatformDto>> GetPlatformByIdAsync(int id)
        {
            var platform = await _repo.GetPlatfomByIdAsync(id);

            if (platform == null)
                return NotFound();

            return Ok(_mapper.Map<PlatformDto>(platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformDto>> CreatePlatform (PlatformCreateDto platformDto)
        {
            if (!TryValidateModel(platformDto))
                throw new ValidationException($"Create: The model of {nameof(platformDto)} is invalid");

            var platformModel = _mapper.Map<Platform>(platformDto);
            _repo.AddPlatform(platformModel);
            await _repo.SaveChangesAsync();

            var platformReturn = _mapper.Map<PlatformDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatformByIdAsync), new { Id = platformReturn.Id }, platformReturn);
        }

        [HttpPut]
        public async Task<ActionResult<PlatformDto>> UpdatePlatform (PlatformDto platformDto)
        {
            if (!TryValidateModel(platformDto))
                throw new ValidationException($"Update: The model of {nameof(platformDto)} is invalid");

            var platformModel = _mapper.Map<Platform>(platformDto);
            _repo.UpdatePlatform(platformModel);
            await _repo.SaveChangesAsync();

            var platformReturn = _mapper.Map<PlatformDto>(platformModel);
            return CreatedAtRoute(nameof(GetPlatformByIdAsync), new { Id = platformReturn.Id }, platformReturn);
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePlatform(PlatformDto platformDto)
        {
            if(!TryValidateModel(platformDto))
                throw new ValidationException($"Delete: The model of {nameof(platformDto)} is invalid");

            var platformModel = _mapper.Map<Platform>(platformDto);
            _repo.DeletePlatform(platformModel);
            await _repo.SaveChangesAsync();

            return Ok();
        }
    }
}
