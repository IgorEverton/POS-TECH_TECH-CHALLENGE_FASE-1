using CadastroNumeros.Domain.Models;
using CadastroNumeros.Domain.ValueObjects;
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
            var email = new Email("carlos.silva@example.com");

            var contato = new Contato
            (
                Guid.NewGuid(),
                new DateTime(),
                "Carlos Silva",
                28,
                "987654321",
                email,
                21
            );

            _mockContatoRepository.Setup(repo => repo.CriarContatoAsync(contato))
                .ReturnsAsync(contato);

            var result = await _contatoService.CriarContatoAsync(contato);

            _mockContatoRepository.Verify(repo => repo.CriarContatoAsync(contato), Times.Once);
            Assert.Equal(contato, result);
        }

        [Fact]
        public async Task RetornarContato_DeveChamarRepositoryERetornarContato()
        {
            var contatoId = Guid.NewGuid();
            var email = new Email("ana.souza@example.com");

            var contato = new Contato
            (
                contatoId,
                new DateTime(),
                "Ana Souza",
                25,
                "123456789",
                email,
                31
            );

            _mockContatoRepository.Setup(repo => repo.RetornarContatoAsync(contatoId))
                .ReturnsAsync(contato);

            var result = await _contatoService.RetornarContatoAsync(contatoId);

            _mockContatoRepository.Verify(repo => repo.RetornarContatoAsync(contatoId), Times.Once);
            Assert.Equal(contato, result);
        }

        [Fact]
        public async Task ListarContatos_DeveChamarRepositoryERetornarTodosOsContatos()
        {
            var email1 = new Email("pedro.lima@example.com");
            var email2 = new Email("maria.silva@example.com");

            var contatos = new List<Contato>
            {
                new Contato ( Guid.NewGuid(), new DateTime(),"Pedro Lima", 30, "111111111", email1, 11 ),
                new Contato ( Guid.NewGuid(), new DateTime(), "Maria Silva", 35, "222222222", email2, 21 )
            };

            _mockContatoRepository.Setup(repo => repo.ListarContatosAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(contatos);

            var result = await _contatoService.ListarContatosAsync(1, 10);

            _mockContatoRepository.Verify(repo => repo.ListarContatosAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(contatos, result);
        }

        [Fact]
        public async Task ListarContatosPorDdd_DeveChamarRepositoryERetornarContatosComOsDDDs()
        {
            var ddd = 11;
            var email1 = new Email("pedro.lima@example.com");
            var email2 = new Email("jose.almeida@example.com");

            var contatos = new List<Contato>
            {
                new Contato ( Guid.NewGuid(), new DateTime(),"Pedro Lima", 30, "111111111", email1, 11 ),
                new Contato ( Guid.NewGuid(), new DateTime(), "José Almeida", 40, "333333333", email2, 11 )
            };

            _mockContatoRepository.Setup(repo => repo.ListarContatosPorDddAsync(ddd))
                .ReturnsAsync(contatos);

            var result = await _contatoService.ListarContatosPorDddAsync(ddd);

            _mockContatoRepository.Verify(repo => repo.ListarContatosPorDddAsync(ddd), Times.Once);
            Assert.Equal(contatos, result);
        }

        [Fact]
        public async Task AtualizarContato_DeveChamarORepository()
        {
            var email = new Email("pedro.lima@example.com");

            var contato = new Contato
            (
                Guid.NewGuid(),
                new DateTime(),
                "Pedro Lima",
                30,
                "111111111",
                email,
                11
            );

            await _contatoService.AtualizarContatoAsync(contato);

            _mockContatoRepository.Verify(repo => repo.AtualizarContatoAsync(contato), Times.Once);
        }

        [Fact]
        public async Task DeletarContato_DeveChamarORepository()
        {
            var contatoId = Guid.NewGuid();

            await _contatoService.DeletarContatoAsync(contatoId);

            _mockContatoRepository.Verify(repo => repo.DeletarContatoAsync(contatoId), Times.Once);
        }
    }
}
