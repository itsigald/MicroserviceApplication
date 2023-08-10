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
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
        }
    }
}
