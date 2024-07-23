using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Services;
using Moq;

namespace CadastroNumeros.Teste.Services
{
    public class ContatoServiceTests
    {
        private MockRepository mockRepository;

        private Mock<IContatoRepository> mockContatoRepository;

        public ContatoServiceTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockContatoRepository = this.mockRepository.Create<IContatoRepository>();
        }

        private ContatoService CreateService()
        {
            return new ContatoService(
                this.mockContatoRepository.Object);
        }

        [Fact]
        public async Task AtualizarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Contato contato = null;

            // Act
            await service.AtualizarContato(
                contato);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task CriarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Contato contato = null;

            // Act
            var result = await service.CriarContato(
                contato);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeletarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Guid Id = new Guid();

            // Act
            await service.DeletarContato(
                Id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ListarContatos_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();

            // Act
            var result = await service.ListarContatos();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task RetornarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var service = this.CreateService();
            Guid id = new Guid();

            // Act
            var result = await service.RetornarContato(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
