using CommandsService.Dtos;
using CommandsService.EventProcessor;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace CommandsService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly ISettingService _setting;
        private readonly ILogger<MessageBusSubscriber> _logger;
        private readonly IEventProcessor _eventProcessor;
        private IConnection? _connection = null;
        private IModel? _channel = null;
        private string _queueName = string.Empty;

        public MessageBusSubscriber(ISettingService setting, ILogger<MessageBusSubscriber> logger, IEventProcessor eventProcessor)
        {
            _setting = setting;
            _logger = logger;
            _eventProcessor = eventProcessor;

            InizializeRabbitMQ();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                _logger.LogInformation("-->Rabbit: Event Received");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _channel.BasicConsume(_queueName, true, consumer);

            return Task.CompletedTask;
        }

        private void InizializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _setting.GetSetting!.RabbitMQ.HostName,
                Port = _setting.GetSetting.RabbitMQ.Port
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, "trigger", "");

            _logger.LogInformation("--> Listening on Message Bus...");

            _connection.ConnectionShutdown += _connection_ConnectionShutdown;
        }

        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            _logger.LogInformation("--> Connection is shutdown");
        }

        public override void Dispose()
        {
            if(_channel != null && _channel.IsClosed)
                _channel.Close();

            if (_connection != null && _connection.IsOpen)
                _connection.Close();

            base.Dispose();
        }
    }
}
