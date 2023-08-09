using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            // Source        -> Target
            // PlatformModel -> Platformdot 

            CreateMap<Platform, PlatformDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformDto, Platform>();
            CreateMap<PlatformDto, PlatformPubDto>();
        }
    }
}
