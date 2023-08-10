using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using Grpc.Net.Client;

namespace CommandsService.SyncDataServices.Grpc
{
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly ISettingService _setting;
        private readonly IMapper _mapper;
        private readonly ILogger<PlatformDataClient> _logger;

        public PlatformDataClient(ISettingService setting, IMapper mapper, ILogger<PlatformDataClient> logger)
        {
            _setting = setting;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<Platform>? ReturnAllPlatform()
        {
            _logger.LogInformation($"--> Calling GRPC Service { _setting.GetSetting!.GrpcPLatformUrl }");
            var channel = GrpcChannel.ForAddress(_setting.GetSetting!.GrpcPLatformUrl);
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var resp = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(resp.Platform);
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Counld not call GRPC server: {ex.Message}");
                return null;
            }
        }
    }
}
