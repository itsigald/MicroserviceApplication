using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Modals;

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
        }
    }
}
