namespace CommandsService.Dtos
{
    public class Setting
    {
        public string CommandServiceUrl { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string GrpcPLatformUrl { get; set; } = string.Empty;
        public RabbitMQConfig RabbitMQ { get; set; } = new();
    }
}
