using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncDataServices.Grpc
{
    public class GrpcPlatformService : GrpcPlatform.GrpcPlatformBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<GrpcPlatformService> _logger;

        public GrpcPlatformService(IPlatformRepo repo, IMapper mapper, ILogger<GrpcPlatformService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async override Task<PlatformResponse> GetAllPlatforms (GetAllRequest request, ServerCallContext context)
        {
            var response = new PlatformResponse();

            var platforms = await _repo.GetAllPlatformsAsync();
            
            foreach ( var platform in platforms )
            {
                response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
            }

            return response;
        }
    }
}
