using CadastroNumeros.Domain.Configuration.CustomMessages;
using CadastroNumeros.Domain.Configuration.IOptions;
using CadastroNumeros.Domain.Configuration.Queues.RabbitMQ;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Repository;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        private readonly IContatoRepository _contatoRepository;
        public ContatoMessageConsumerService(IOptions<RabbitMQSetting> rabbitMqSetting,
                                             IContatoRepository contatoRepository, 
                                             ILogger<ContatoMessageConsumerService> logger)
        {
            _rabbitMqSetting = rabbitMqSetting.Value;
            _logger = logger;
            _contatoRepository = contatoRepository;

            _factory = new ConnectionFactory
            {
                HostName = _rabbitMqSetting.HostName,
                UserName = _rabbitMqSetting.UserName,
                Password = _rabbitMqSetting.Password
            };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection = await _factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();

            await StartConsuming(RabbitMQQueues.CadastroContatoQueue, stoppingToken);
            await Task.CompletedTask;
        }

        private async Task StartConsuming(string queueName, CancellationToken cancellationToken)
        {
            _logger.LogInformation(QueueProcessingMessages.ConsumoIniciado);

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
                    _logger.LogError($"{QueueProcessingMessages.ErroProcessarQueue} {queueName}: {ex}");
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

        public async Task<bool> ProcessMessageAsync(string message)
        {
            try
            {
                var solicitacao = JsonConvert.DeserializeObject<CadastroSolicitacao>(message);

                switch (solicitacao.TipoSolicitacao)
                {
                    case Domain.Enum.TipoSolicitacao.Inserir:
                        await _contatoRepository.CriarContato(solicitacao.Contato);
                        break;
                    case Domain.Enum.TipoSolicitacao.Alterar:
                        await _contatoRepository.AtualizarContato(solicitacao.Contato);
                        break;
                    case Domain.Enum.TipoSolicitacao.Deletar:
                        await _contatoRepository.DeletarContato(solicitacao.Contato.Id);
                        break;
                    default:
                        _logger.LogWarning(QueueProcessingMessages.AcaoInvalida);
                        return false;
                }

                _logger.LogInformation(QueueProcessingMessages.OperacaoRealizadaSucesso);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }

        }

        public override void Dispose()
        {
            if(_channel is not null)
                _channel.CloseAsync();

            if(_connection is not null)
                _connection.CloseAsync();

            base.Dispose();
        }
    }
}
