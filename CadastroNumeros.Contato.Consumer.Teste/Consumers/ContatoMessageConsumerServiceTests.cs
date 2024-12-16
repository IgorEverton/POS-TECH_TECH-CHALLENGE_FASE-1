using CadastroNumeros.Contato.Consumer.Consumers;
using CadastroNumeros.Domain.Configuration.CustomMessages;
using CadastroNumeros.Domain.Configuration.IOptions;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;

namespace CadastroNumeros.Contato.Consumer.Teste.Consumers
{
    public class ContatoMessageConsumerServiceTests
    {
        private readonly Mock<IOptions<RabbitMQSetting>> _mockRabbitMqSetting;
        private readonly Mock<IContatoRepository> _mockContatoRepository;
        private readonly Mock<ILogger<ContatoMessageConsumerService>> _mockLogger;
        private readonly RabbitMQSetting _rabbitMqSetting;

        public ContatoMessageConsumerServiceTests()
        {
            _mockRabbitMqSetting = new Mock<IOptions<RabbitMQSetting>>();
            _mockContatoRepository = new Mock<IContatoRepository>();
            _mockLogger = new Mock<ILogger<ContatoMessageConsumerService>>();

            _rabbitMqSetting = new RabbitMQSetting
            {
                HostName = "localhost",
                UserName = "user",
                Password = "password"
            };

            _mockRabbitMqSetting.Setup(x => x.Value).Returns(_rabbitMqSetting);
        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldLogInformation_WhenOperationSucceeds()
        {
            // Arrange
            var solicitacao = new CadastroSolicitacao
            {
                TipoSolicitacao = Domain.Enum.TipoSolicitacao.Inserir,
                Contato = new Domain.Models.Contato { Id = Guid.NewGuid(), Nome = "Test Name" }
            };
            var message = JsonConvert.SerializeObject(solicitacao);

            _mockContatoRepository
                .Setup(repo => repo.CriarContato(It.IsAny<Domain.Models.Contato>()))
                .Returns(Task.FromResult(It.IsAny<Domain.Models.Contato>()));

            var service = new ContatoMessageConsumerService(
                _mockRabbitMqSetting.Object,
                _mockContatoRepository.Object,
                _mockLogger.Object
            );

            // Act
            var result = await service.ProcessMessageAsync(message);

            // Assert
            Assert.True(result);

            _mockLogger.Verify(
                log => log.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Information),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains(QueueProcessingMessages.OperacaoRealizadaSucesso)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public async Task ProcessMessageAsync_ShouldLogWarning_WhenInvalidActionIsReceived()
        {
            // Arrange
            var solicitacao = new CadastroSolicitacao
            {
                TipoSolicitacao = (Domain.Enum.TipoSolicitacao)99, // Tipo inválido
                Contato = new Domain.Models.Contato { Id = Guid.NewGuid(), Nome = "Test Name" }
            };
            var message = JsonConvert.SerializeObject(solicitacao);

            var service = new ContatoMessageConsumerService(
                _mockRabbitMqSetting.Object,
                _mockContatoRepository.Object,
                _mockLogger.Object
            );

            // Act
            var result = await service.ProcessMessageAsync(message);

            // Assert
            Assert.False(result);

            _mockLogger.Verify(
                log => log.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Warning),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == "Tipo de ação invalido"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }




        [Fact]
        public async Task ProcessMessageAsync_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var solicitacao = new CadastroSolicitacao
            {
                TipoSolicitacao = Domain.Enum.TipoSolicitacao.Inserir,
                Contato = new CadastroNumeros.Domain.Models.Contato { Id = Guid.NewGuid(), Nome = "Test Name" }
            };
            var message = JsonConvert.SerializeObject(solicitacao);

            _mockContatoRepository
                .Setup(repo => repo.CriarContato(It.IsAny<CadastroNumeros.Domain.Models.Contato>()))
                .ThrowsAsync(new Exception("Test Exception"));

            var service = new ContatoMessageConsumerService(
                _mockRabbitMqSetting.Object,
                _mockContatoRepository.Object,
                _mockLogger.Object
            );

            // Act
            var result = await service.ProcessMessageAsync(message);

            // Assert
            Assert.False(result);

            _mockLogger.Verify(
                log => log.Log(
                    It.Is<LogLevel>(level => level == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString().Contains("Test Exception")),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

    }
}
