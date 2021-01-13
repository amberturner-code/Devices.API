namespace Devices.API.Domain
{
    public class RabbitMQConfiguration
    {
        public string Hostname { get; set; }

        public string RequestQueueName { get; set; }

        public string ResponseQueueName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Enabled { get; set; }
    }
}
