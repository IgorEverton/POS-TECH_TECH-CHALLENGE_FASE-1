using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Queues;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Services;
using Moq;

namespace CadastroNumeros.Teste.Services
{
    public class ContatoServiceTests
    {
        private readonly Mock<IContatoRepository> _mockContatoRepository;
        private readonly ContatoService _contatoService;
        private readonly Mock<IRabbitMQPublisher<Contato>> _rabbitPublisher;

        public ContatoServiceTests()
        {
            _mockContatoRepository = new Mock<IContatoRepository>();
            _rabbitPublisher = new Mock<IRabbitMQPublisher<Contato>>();
            _contatoService = new ContatoService(_mockContatoRepository.Object, _rabbitPublisher.Object);
        }

        [Fact]
        public async Task CriarContato_DeveChamarRepositoryERetornarContato()
        {
            var contato = new Contato
            {
                Id = Guid.NewGuid(),
                Nome = "Carlos Silva",
                Idade = 28,
                Email = "carlos.silva@example.com",
                Telefone = "987654321",
                CodigoDdd = 21
            };

            _mockContatoRepository.Setup(repo => repo.CriarContato(contato))
                .ReturnsAsync(contato);

            var result = await _contatoService.CriarContato(contato);

            _mockContatoRepository.Verify(repo => repo.CriarContato(contato), Times.Once);
            Assert.Equal(contato, result);
        }

        [Fact]
        public async Task RetornarContato_DeveChamarRepositoryERetornarContato()
        {
            var contatoId = Guid.NewGuid();
            var contato = new Contato
            {
                Id = contatoId,
                Nome = "Ana Souza",
                Idade = 25,
                Email = "ana.souza@example.com",
                Telefone = "123456789",
                CodigoDdd = 31
            };

            _mockContatoRepository.Setup(repo => repo.RetornarContato(contatoId))
                .ReturnsAsync(contato);

            var result = await _contatoService.RetornarContato(contatoId);

            _mockContatoRepository.Verify(repo => repo.RetornarContato(contatoId), Times.Once);
            Assert.Equal(contato, result);
        }

        [Fact]
        public async Task ListarContatos_DeveChamarRepositoryERetornarTodosOsContatos()
        {
            var contatos = new List<Contato>
            {
                new Contato { Id = Guid.NewGuid(), Nome = "Pedro Lima", Idade = 30, Email = "pedro.lima@example.com", Telefone = "111111111", CodigoDdd = 11 },
                new Contato { Id = Guid.NewGuid(), Nome = "Maria Silva", Idade = 35, Email = "maria.silva@example.com", Telefone = "222222222", CodigoDdd = 21 }
            };

            _mockContatoRepository.Setup(repo => repo.ListarContatos(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(contatos);

            var result = await _contatoService.ListarContatos(1, 10);

            _mockContatoRepository.Verify(repo => repo.ListarContatos(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(contatos, result);
        }

        [Fact]
        public async Task ListarContatosPorDdd_DeveChamarRepositoryERetornarContatosComOsDDDs()
        {
            var ddd = 11;
            var contatos = new List<Contato>
            {
                new Contato { Id = Guid.NewGuid(), Nome = "Pedro Lima", Idade = 30, Email = "pedro.lima@example.com", Telefone = "111111111", CodigoDdd = 11 },
                new Contato { Id = Guid.NewGuid(), Nome = "José Almeida", Idade = 40, Email = "jose.almeida@example.com", Telefone = "333333333", CodigoDdd = 11 }
            };

            _mockContatoRepository.Setup(repo => repo.ListarContatosPorDdd(ddd))
                .ReturnsAsync(contatos);

            var result = await _contatoService.ListarContatosPorDdd(ddd);

            _mockContatoRepository.Verify(repo => repo.ListarContatosPorDdd(ddd), Times.Once);
            Assert.Equal(contatos, result);
        }

        [Fact]
        public async Task AtualizarContato_DeveChamarORepository()
        {
            var contato = new Contato
            {
                Id = Guid.NewGuid(),
                Nome = "Pedro Lima",
                Idade = 30,
                Email = "pedro.lima@example.com",
                Telefone = "111111111",
                CodigoDdd = 11
            };

            await _contatoService.AtualizarContato(contato);

            _mockContatoRepository.Verify(repo => repo.AtualizarContato(contato), Times.Once);
        }

        [Fact]
        public async Task DeletarContato_DeveChamarORepository()
        {
            var contatoId = Guid.NewGuid();

            await _contatoService.DeletarContato(contatoId);

            _mockContatoRepository.Verify(repo => repo.DeletarContato(contatoId), Times.Once);
        }
    }
}
