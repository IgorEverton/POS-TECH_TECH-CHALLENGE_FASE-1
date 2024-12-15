using CadastroNumeros.Domain.Configuration.CustomMessages;
using CadastroNumeros.Domain.Configuration.Queues.RabbitMQ;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Polices;
using CadastroNumeros.Infra.Interfaces.Queues;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Services;
using Moq;
using Polly;
using Polly.CircuitBreaker;

namespace CadastroNumeros.Teste.Services
{
    public class ContatoServiceTests
    {
        private readonly Mock<IContatoRepository> _mockRepository;
        private readonly Mock<IRabbitMQPublisher<CadastroSolicitacao>> _mockPublisher;
        private readonly Mock<ICircuitBreakerPolicyProvider> _mockPolicyProvider;
        private readonly AsyncCircuitBreakerPolicy _mockPolicy;

        private readonly ContatoService _service;

        public ContatoServiceTests()
        {
            _mockRepository = new Mock<IContatoRepository>();
            _mockPublisher = new Mock<IRabbitMQPublisher<CadastroSolicitacao>>();
            _mockPolicyProvider = new Mock<ICircuitBreakerPolicyProvider>();

            // Configura a política simulada
            _mockPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)); // Define um circuito que quebra após 3 falhas

            // Configura o mock para retornar a política simulada
            _mockPolicyProvider.Setup(p => p.GetPolicy()).Returns(_mockPolicy);

            // Instancia o serviço com os mocks
            _service = new ContatoService(
                _mockRepository.Object,
                _mockPublisher.Object,
                _mockPolicyProvider.Object);
        }

        // Teste de sucesso
        [Fact]
        public async Task CriarContato_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var contato = new Contato { Id = Guid.NewGuid(), Nome = "Teste", Telefone = "999999999" };

            // Configura o mock do publisher para retornar sucesso
            _mockPublisher
                .Setup(p => p.PublishMessageAsync(It.IsAny<CadastroSolicitacao>(), RabbitMQQueues.CadastroContatoQueue))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CriarContato(contato);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal(ReturnMessages.SolicitacaoRealizada, result.Mensagem);
        }

        [Fact]
        public async Task CriarContato_CircuitBreakerOpen_ReturnsFailureMessage()
        {
            // Arrange
            var contato = new Contato { Id = Guid.NewGuid(), Nome = "Teste", Telefone = "999999999" };

            // Configura o mock do publisher para lançar uma exceção quando o Circuit Breaker está aberto
            _mockPublisher
                .Setup(p => p.PublishMessageAsync(It.IsAny<CadastroSolicitacao>(), RabbitMQQueues.CadastroContatoQueue))
                .ThrowsAsync(new BrokenCircuitException("Circuito aberto"));

            // Simula o Circuit Breaker já "aberto"
            var policy = _mockPolicyProvider.Object.GetPolicy();

            // Act
            var result = await _service.CriarContato(contato);

            // Assert
            Assert.False(result.Sucesso);
            Assert.Contains("O serviço está temporariamente indisponível", result.Mensagem);
        }



        // Teste para o método AtualizarContato
        [Fact]
        public async Task AtualizarContato_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var contato = new Contato { Id = Guid.NewGuid(), Nome = "Teste", Telefone = "999999999" };

            // Configura o mock do publisher para retornar sucesso
            _mockPublisher
                .Setup(p => p.PublishMessageAsync(It.IsAny<CadastroSolicitacao>(), RabbitMQQueues.CadastroContatoQueue))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.AtualizarContato(contato);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal(ReturnMessages.SolicitacaoRealizada, result.Mensagem);
        }

        // Teste para o método DeletarContato
        [Fact]
        public async Task DeletarContato_Success_ReturnsSuccessMessage()
        {
            // Arrange
            var contato = new Contato { Id = Guid.NewGuid(), Nome = "Teste", Telefone = "999999999" };

            // Configura o mock do publisher para retornar sucesso
            _mockPublisher
                .Setup(p => p.PublishMessageAsync(It.IsAny<CadastroSolicitacao>(), RabbitMQQueues.CadastroContatoQueue))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeletarContato(contato.Id);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal(ReturnMessages.SolicitacaoRealizada, result.Mensagem);
        }
    }
}