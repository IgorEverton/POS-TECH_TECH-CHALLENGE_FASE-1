using CadastroNumeros.Domain.Configuration.IOptions;
using CadastroNumeros.Infra.Interfaces.Queues;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace CadastroNumeros.Infra.Queues.RabbitMQ
{
    public class RabbitMQPublisher<T> : IRabbitMQPublisher<T>
    {
        private readonly RabbitMQSetting _rabbitMqSetting;

        public RabbitMQPublisher(IOptions<RabbitMQSetting> rabbitMqSetting)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
        }

        public async Task PublishMessageAsync(T message, string queueName)
        {

            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            var props = new BasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = DeliveryModes.Persistent;

            await channel.BasicPublishAsync("", queueName, mandatory: true, basicProperties: props, body: body);
        }
    }
}
