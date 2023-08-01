using PlatformService.Dtos;
using PlatformService.Services;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<HttpCommandDataClient> _logger;
        private readonly ISettingService _config;

        public HttpCommandDataClient(HttpClient client, ILogger<HttpCommandDataClient> logger, ISettingService config)
        {
            _client = client;
            _logger = logger;
            _config = config;
        }

        public async Task SendPlatformToCommando(PlatformDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _client.PostAsync($"{ _config.GetSetting?.CommandServiceUrl }/api/c/Platforms/", httpContent);

            if(response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Sync POST to CommandService was OK");
            }
            else
            {
                _logger.LogError("Sync POST to CommandService was KO");
            }
        }
    }
}
