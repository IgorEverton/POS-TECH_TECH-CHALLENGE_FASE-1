using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Queues;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Services;
using Moq;

namespace CadastroNumeros.Teste.Services
{
    using CadastroNumeros.Domain.Configuration.Queues.RabbitMQ;
    using CadastroNumeros.Domain.Configuration.CustomMessages;
    using CadastroNumeros.Domain.Models;
    using CadastroNumeros.Domain.Enum;
    using CadastroNumeros.Infra.Interfaces.Queues;
    using CadastroNumeros.Infra.Interfaces.Repository;
    using CadastroNumeros.Infra.Services;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class ContatoServiceTests
    {
        private readonly Mock<IContatoRepository> _mockContatoRepository;
        private readonly Mock<IRabbitMQPublisher<CadastroSolicitacao>> _mockRabbitMQPublisher;
        private readonly ContatoService _contatoService;

        public ContatoServiceTests()
        {
            _mockContatoRepository = new Mock<IContatoRepository>();
            _mockRabbitMQPublisher = new Mock<IRabbitMQPublisher<CadastroSolicitacao>>();
            _contatoService = new ContatoService(_mockContatoRepository.Object, _mockRabbitMQPublisher.Object);
        }

        [Fact]
        public async Task CriarContato_ShouldReturnSuccess_WhenMessageIsPublished()
        {
            // Arrange
            var contato = new Contato { Id = Guid.NewGuid(), Nome = "Teste", CodigoDdd = 11, Telefone = "999999999" };

            _mockRabbitMQPublisher
                .Setup(p => p.PublishMessageAsync(It.IsAny<CadastroSolicitacao>(), RabbitMQQueues.CadastroContatoQueue))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _contatoService.CriarContato(contato);

            // Assert
            Assert.True(result.Sucesso);
            Assert.Equal(ReturnMessages.SolicitacaoRealizada, result.Mensagem);
            _mockRabbitMQPublisher.Verify(p => p.PublishMessageAsync(
                It.Is<CadastroSolicitacao>(s => s.Contato == contato && s.TipoSolicitacao == TipoSolicitacao.Inserir),
                RabbitMQQueues.CadastroContatoQueue), Times.Once);
        }

        [Fact]
        public async Task CriarContato_ShouldReturnFailure_WhenExceptionOccurs()
        {
            // Arrange
            var contato = new Contato { Id = Guid.NewGuid(), Nome = "Teste" };

            _mockRabbitMQPublisher
                .Setup(p => p.PublishMessageAsync(It.IsAny<CadastroSolicitacao>(), RabbitMQQueues.CadastroContatoQueue))
                .ThrowsAsync(new Exception("Erro no RabbitMQ"));

            // Act
            var result = await _contatoService.CriarContato(contato);

            // Assert
            Assert.False(result.Sucesso);
            Assert.StartsWith(ReturnMessages.SolicitacaoNaoRealizada, result.Mensagem);
        }

        [Fact]
        public async Task ListarContatos_ShouldReturnListFromRepository()
        {
            // Arrange
            var contatos = new List<Contato>
        {
            new Contato { Id = Guid.NewGuid(), Nome = "Contato 1" },
            new Contato { Id = Guid.NewGuid(), Nome = "Contato 2" }
        };

            _mockContatoRepository
                .Setup(r => r.ListarContatos(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(contatos);

            // Act
            var result = await _contatoService.ListarContatos();

            // Assert
            Assert.Equal(contatos, result);
            _mockContatoRepository.Verify(r => r.ListarContatos(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task RetornarContato_ShouldReturnContato_WhenExists()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var contato = new Contato { Id = contatoId, Nome = "Teste" };

            _mockContatoRepository
                .Setup(r => r.RetornarContato(contatoId))
                .ReturnsAsync(contato);

            // Act
            var result = await _contatoService.RetornarContato(contatoId);

            // Assert
            Assert.Equal(contato, result);
            _mockContatoRepository.Verify(r => r.RetornarContato(contatoId), Times.Once);
        }

        [Fact]
        public async Task RetornarContato_ShouldReturnNull_WhenNotFound()
        {
            // Arrange
            var contatoId = Guid.NewGuid();

            _mockContatoRepository
                .Setup(r => r.RetornarContato(contatoId))
                .ReturnsAsync((Contato)null);

            // Act
            var result = await _contatoService.RetornarContato(contatoId);

            // Assert
            Assert.Null(result);
        }
    }

}
