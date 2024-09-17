using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CadastroNumeros.Tests.Integration
{
    public class AppDbContextIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;

        public AppDbContextIntegrationTests()
        {
            // Obter a string de conexão da variável de ambiente
            var connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("A variável de ambiente SQLSERVER_CONNECTION_STRING não está configurada.");
            }

            // Configurando o DbContext para usar a string de conexão vinda da variável de ambiente
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new AppDbContext(options);

            // Garantir que o banco de dados está criado
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task CanInsertContatoIntoDatabase()
        {
            // Arrange: criar uma nova entidade Contato
            var contato = new Contato
            {
                Id = Guid.NewGuid(),
                DataCriacao = DateTime.UtcNow,
                Nome = "João Silva",
                Idade = 30,
                Email = "joao.silva@email.com",
                Telefone = "123456789",
                CodigoDdd = 11
            };

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

        // Limpeza do banco de dados após cada teste
        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Apagar o banco de dados para evitar conflitos em testes futuros
            _context.Dispose();
        }
    }
}

