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
            var nome = "João Silva";
            var idade = 30;
            var email = new Email("joao.silva@email.com");
            var telefone = "123456789";
            var codigoDdd = 11;
            // Arrange: criar uma nova entidade Contato
            var contato = new Contato
            (
                Guid.NewGuid(),
                DateTime.UtcNow,
                nome,
                idade,
                telefone,
                email,
                codigoDdd
            );

            // Act: adicionar e salvar a entidade no banco de dados
            await _context.Contatos.AddAsync(contato);
            await _context.SaveChangesAsync();

            // Assert: verificar se o contato foi inserido corretamente
            var contatoInserido = await _context.Contatos.FirstOrDefaultAsync(c => c.Email == email);
            Assert.NotNull(contatoInserido);
            Assert.Equal(nome, contatoInserido.Nome);
            Assert.Equal(idade, contatoInserido.Idade);
            Assert.Equal(telefone, contatoInserido.Telefone);
            Assert.Equal(codigoDdd, contatoInserido.CodigoDdd);
            Assert.Equal(contato.DataCriacao, contatoInserido.DataCriacao);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Apagar o banco de dados para evitar conflitos em testes futuros
            _context.Dispose();
        }
    }
}

