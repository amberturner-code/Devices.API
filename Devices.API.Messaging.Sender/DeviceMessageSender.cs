using Devices.API.Domain;
using Devices.API.Domain.Models;
using Devices.API.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Devices.API.Messaging.Sender
{
    public class DeviceMessageSender : IDeviceMessageSender
    {
        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private readonly ILoggerService _logger;
        private IConnection _connection;

        public DeviceMessageSender(IOptions<RabbitMQConfiguration> rabbitMqOptions, ILoggerService logger)
        {
            _queueName = rabbitMqOptions.Value.RequestQueueName;
            _hostname = rabbitMqOptions.Value.Hostname;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            _logger = logger;

            CreateConnection();
        }

        public void SendMessage(Device device)
        {
            if (ConnectionExists())
            {
                using (var channel = _connection.CreateModel())
                {
                    channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    channel.ConfirmSelect();
                    channel.BasicAcks += (sender, eventArgs) =>
                    {                        
                        HandleAck(sender, eventArgs);
                    };

                    var json = JsonConvert.SerializeObject(device);
                    var body = Encoding.UTF8.GetBytes(json);

                    channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                }
            }
        }

        private void HandleAck(object sender, BasicAckEventArgs args)
        {
            _logger.LogInfo($"Assign request acknowledged in Devices.API: sender = {sender} message: {args.DeliveryTag}");
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }
    }
}
