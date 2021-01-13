using Devices.API.Domain;
using Devices.API.Domain.Models;
using Devices.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devices.API.Messaging.Receiver
{
    public class DeviceMessageReceiver : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private readonly string _hostname;
        private readonly string _queueName;
        private readonly string _username;
        private readonly string _password;

        public IServiceProvider Services { get; }

        public DeviceMessageReceiver(IServiceProvider services, IOptions<RabbitMQConfiguration> rabbitMqOptions)
        {
            _hostname = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.ResponseQueueName;
            _username = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;
            Services = services;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            _connection = factory.CreateConnection();            
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var device = JsonConvert.DeserializeObject<Device>(content);

                HandleMessage(device);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            
            _channel.BasicConsume(_queueName, false, consumer);

            return Task.CompletedTask;
        }

        private async void HandleMessage(Device device)
        {
            using (var scope = Services.CreateScope())
            {
                var deviceService =
                    scope.ServiceProvider
                        .GetRequiredService<IDeviceService>();

                await deviceService.UpdateDeviceStatus(device);
            }
        }        

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
