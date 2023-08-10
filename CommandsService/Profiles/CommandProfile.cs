using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.Profiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            // Source        -> Target
            // PlatformModel -> Platformdot 

            CreateMap<Platform, PlatformDto>();
            CreateMap<CommandDto, Command>();
            CreateMap<Command, CommandDto>();
            //
            CreateMap<CommandCreateDto, Command>();
            CreateMap<PlatformCreateDto, PlatformPubDto>();
            //
            CreateMap<PlatformPubDto, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));

            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Commands, opt => opt.Ignore());
        }
    }
}
