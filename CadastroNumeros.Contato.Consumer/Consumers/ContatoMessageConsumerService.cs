using CadastroNumeros.Domain.Configuration.IOptions;
using CadastroNumeros.Domain.Configuration.Queues.RabbitMQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CadastroNumeros.Contato.Consumer.Consumers
{
    public class ContatoMessageConsumerService : BackgroundService
    {
        private readonly ILogger<ContatoMessageConsumerService> _logger;
        private readonly RabbitMQSetting _rabbitMqSetting;
        private IConnection _connection;
        private IChannel _channel;
        private readonly ConnectionFactory _factory;
        public ContatoMessageConsumerService(IOptions<RabbitMQSetting> rabbitMqSetting, IServiceProvider serviceProvider, ILogger<ContatoMessageConsumerService> logger)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
            _logger = logger;

            _factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Iniciando conexão com a fila");

            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            Console.WriteLine("Fila conectada iniciando consumo!");

            await StartConsuming(RabbitMQQueues.CadastroContatoQueue, stoppingToken);
            await Task.CompletedTask;
        }

        private async Task StartConsuming(string queueName, CancellationToken cancellationToken)
        {
            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                bool processedSuccessfully = false;

                try
                {
                    processedSuccessfully = await ProcessMessageAsync(message);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Exception occurred while processing message from queue {queueName}: {ex}");
                }

                if (processedSuccessfully)
                {
                    await _channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    await _channel.BasicRejectAsync(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }

        private async Task<bool> ProcessMessageAsync(string message)
        {
            Console.WriteLine(message);

            return true;
        }

        public override void Dispose()
        {
            _channel.CloseAsync();
            _connection.CloseAsync();
            base.Dispose();
        }
    }
}
