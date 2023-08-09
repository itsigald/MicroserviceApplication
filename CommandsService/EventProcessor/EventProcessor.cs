using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Text.Json;

namespace CommandsService.EventProcessor
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        private readonly ILogger<EventProcessor> _logger;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper, ILogger<EventProcessor> logger)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
            _logger = logger;
        }

        public async void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.PlatformPublished:
                    await AddPlatform(message);
                    break;
                default: 
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            _logger.LogInformation($"--> Determining Event");
            var eventype = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            if(eventype is not null)
            {
                switch (eventype.Event)
                {
                    case "Platoform_Published":
                        _logger.LogInformation($"--> Platoform_Published detected");
                        return EventType.PlatformPublished;
                    default:
                        _logger.LogInformation($"--> Could not determine event type message");
                        return EventType.Undeterminated;
                }
            }
            else
            {
                _logger.LogInformation($"--> Could not determine eventype object");
                return EventType.Undeterminated;
            }
        }

        private async Task AddPlatform(string platformPublishedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPubDto = JsonSerializer.Deserialize<PlatformPubDto>(platformPublishedMessage);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPubDto);
                    var platformExists = await repo.ExternalPlatformExists(platform.ExternalId);
                    
                    if (!platformExists)
                    {
                        platform.Id = 0;
                        platform.Commands = null;
                        repo.CreatePlatform(platform);
                        await repo.SaveChangesAsync();
                        _logger.LogInformation($"--> New Platoform Added: { JsonSerializer.Serialize(platform) }");
                    }
                    else
                    {
                        _logger.LogWarning($"--> Platoform already exists: {platform.ExternalId}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"--> Cound not add Platoform to DB: {ex.Message}");
                }
            }
        }
    }
    enum EventType
    {
        Undeterminated = 0,
        PlatformPublished = 1,
    }
}
