using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Services;
using Moq;

namespace CadastroNumeros.Teste.Services
{
    public class ContatoServiceTests
    {
        private readonly Mock<IContatoRepository> _mockContatoRepository;
        private readonly ContatoService _contatoService;

        public ContatoServiceTests()
        {
            _mockContatoRepository = new Mock<IContatoRepository>();
            _contatoService = new ContatoService(_mockContatoRepository.Object);
        }

        [Fact]
        public async Task CriarContato_DeveChamarRepositoryERetornarContato()
        {
            var contato = new Contato
            (
                Guid.NewGuid(),
                new DateTime(),
                "Carlos Silva",
                28,
                "carlos.silva@example.com",
                "987654321",
                21
            );

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
            (
                contatoId,
                new DateTime(),
                "Ana Souza",
                25,
                "ana.souza@example.com",
                "123456789",
                31
            );

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
                new Contato ( Guid.NewGuid(), new DateTime(),"Pedro Lima", 30, "pedro.lima@example.com", "111111111", 11 ),
                new Contato ( Guid.NewGuid(), new DateTime(), "Maria Silva", 35, "maria.silva@example.com", "222222222", 21 )
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
                new Contato ( Guid.NewGuid(), new DateTime(),"Pedro Lima", 30, "pedro.lima@example.com", "111111111", 11 ),
                new Contato ( Guid.NewGuid(), new DateTime(), "José Almeida", 40, "jose.almeida@example.com", "333333333", 11 )
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
            (
                Guid.NewGuid(),
                new DateTime(),
                "Pedro Lima",
                30,
                "pedro.lima@example.com",
                "111111111",
                11
            );

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
