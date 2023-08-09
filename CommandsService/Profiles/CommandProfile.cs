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
            //
            CreateMap<PlatformPubDto, Platform>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
