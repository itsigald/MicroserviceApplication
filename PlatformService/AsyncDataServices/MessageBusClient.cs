using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Services;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly ISettingService _setting;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<MessageBusClient> _logger;
        private readonly IConnection? _connection = null;
        private readonly IModel? _channel = null;

        public MessageBusClient(ISettingService setting, IWebHostEnvironment env, ILogger<MessageBusClient> logger)
        {
            _setting = setting;
            _env = env;
            _logger = logger;

            var rabbitConnectionFactory = new ConnectionFactory()
            {
                HostName = _setting.GetSetting!.RabbitMQ.HostName,
                Port = _setting.GetSetting.RabbitMQ.Port
            };

            try
            {
                _connection = rabbitConnectionFactory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
                _connection.ConnectionShutdown += Connection_ConnectionShutdown;

                _logger.LogInformation($"--> Connected to RabbitMQ");
            }
            catch (Exception ex)
            {
                _logger.LogError($"--> Could not connect with Rabbit Server: { ex.Message }");
            }
        }

        public void PublishNewPlatform(PlatformPubDto platform)
        {
            var message = JsonSerializer.Serialize(platform);

            if(_connection != null && _connection.IsOpen)
            {
                _logger.LogInformation($"--> Rabbit: sending message...");
                SendMessage(message);
            }
            else
            {
                _logger.LogWarning($"--> Rabbit: connection is closed, not sending message");
            }
        }

        public void Dispose()
        {
            if(_channel != null && _channel.IsOpen)
                _channel.Close();

            if(_connection != null && _connection.IsOpen)
                _connection.Close();
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);
            _logger.LogInformation($"--> Rabbit: the message is sent");
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation($"--> Rabbit: Connection Shutdown.");
        }
    }
}
