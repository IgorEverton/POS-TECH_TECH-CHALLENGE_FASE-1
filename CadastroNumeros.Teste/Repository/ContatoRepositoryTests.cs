using CadastroNumeros.Domain.Models;
using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Data;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CadastroNumeros.Teste.Repository
{
    public class ContatoRepositoryTests
    {
        private MockRepository mockRepository;

        private Mock<AppDbContext> mockAppDbContext;

        public ContatoRepositoryTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockAppDbContext = this.mockRepository.Create<AppDbContext>();
        }

        private ContatoRepository CreateContatoRepository()
        {
            return new ContatoRepository(
                this.mockAppDbContext.Object);
        }

        [Fact]
        public async Task CriarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var contatoRepository = this.CreateContatoRepository();
            Contato contato = null;

            // Act
            var result = await contatoRepository.CriarContato(
                contato);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task RetornarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var contatoRepository = this.CreateContatoRepository();
            Guid id = new Guid();

            // Act
            var result = await contatoRepository.RetornarContato(
                id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task ListarContatos_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var contatoRepository = this.CreateContatoRepository();

            // Act
            var result = await contatoRepository.ListarContatos();

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task AtualizarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var contatoRepository = this.CreateContatoRepository();
            Contato contato = null;

            // Act
            await contatoRepository.AtualizarContato(
                contato);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public async Task DeletarContato_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var contatoRepository = this.CreateContatoRepository();
            Guid Id = new Guid();

            // Act
            await contatoRepository.DeletarContato(
                Id);

            // Assert
            Assert.True(false);
            this.mockRepository.VerifyAll();
        }
    }
}
