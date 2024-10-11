using CadastroNumeros.Domain.Models;
using CadastroNumeros.Domain.ValueObjects;
using CadastroNumeros.Infra.Data;
using CadastroNumeros.Teste.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CadastroNumeros.Tests.Integration
{
    public class AppDbContextIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;        

        public AppDbContextIntegrationTests()
        {
            var options = DbOptionsCreator.CreateDbOptions();

            _context = new AppDbContext(options);

            // Garantir que o banco de dados está criado
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task CanInsertContatoIntoDatabase()
        {
            // Arrange: criar uma nova entidade Contato
            var contato = new Contato
            (
                Guid.NewGuid(),
                DateTime.UtcNow,
                "João Silva",
                30,
                "joao.silva@email.com",
                "123456789",
                11
            );

            // Act: adicionar e salvar a entidade no banco de dados
            await _context.Contatos.AddAsync(contato);
            await _context.SaveChangesAsync();

            // Assert: verificar se o contato foi inserido corretamente
            var contatoInserido = await _context.Contatos.FirstOrDefaultAsync(c => c.Email == "joao.silva@email.com");
            Assert.NotNull(contatoInserido);
            Assert.Equal("João Silva", contatoInserido.Nome);
            Assert.Equal(30, contatoInserido.Idade);
            Assert.Equal("123456789", contatoInserido.Telefone);
            Assert.Equal(11, contatoInserido.CodigoDdd);
            Assert.Equal(contato.DataCriacao, contatoInserido.DataCriacao);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Apagar o banco de dados para evitar conflitos em testes futuros
            _context.Dispose();
        }
    }
}

