using CadastroNumeros.Domain.Models;
using CadastroNumeros.Domain.ValueObjects;
using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Data;
using FluentAssertions.Equivalency;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace CadastroNumeros.Teste.Repository
{
    public class ContatoRepositoryTests
    {
        private DbContextOptions<AppDbContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CriarContato_DeveAdicionarContatoNaDatabase()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new ContatoRepository(context);
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

                var result = await repository.CriarContatoAsync(contato);

                Assert.NotNull(result);
                Assert.Equal("Carlos Silva", result.Nome);
                Assert.Equal(1, context.Contatos.Count());
                Assert.Equal("Carlos Silva", context.Contatos.Single().Nome);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task RetornarContato_DeveRetornarOContatoPeloId()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new ContatoRepository(context);
                var email = new Email("ana.souza@example.com");

                var contato = new Contato
                (
                    Guid.NewGuid(),
                    new DateTime(),
                    "Ana Souza",
                    25,
                    "123456789",
                    email,
                    31
                );

                context.Contatos.Add(contato);
                context.SaveChanges();

                var result = await repository.RetornarContatoAsync(contato.Id);

                Assert.NotNull(result);
                Assert.Equal("Ana Souza", result.Nome);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ListarContatos_DeveRetornarTodosOsContatos()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new ContatoRepository(context);
                var email1 = new Email("pedro.lima@example.com");
                var email2 = new Email("maria.silva@example.com");

                var contatos = new List<Contato>
                {
                    new Contato ( Guid.NewGuid(), new DateTime(), "Pedro Lima", 30, "111111111", email1, 11 ),
                    new Contato ( Guid.NewGuid(), new DateTime(), "Maria Silva", 35, "222222222", email2, 21 )
                };

                context.Contatos.AddRange(contatos);
                context.SaveChanges();

                var result = await repository.ListarContatosAsync(1, 10);

                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ListarContatosPorDdd_DeveRetornarOsContatosFiltrandoPeloDdd()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new ContatoRepository(context);
                var email1 = new Email("pedro.lima@example.com");
                var email2 = new Email("maria.silva@example.com");
                var email3 = new Email("jose.almeida@example.com");

                var contatos = new List<Contato>
                {
                    new Contato ( Guid.NewGuid(), new DateTime(), "Pedro Lima", 30, "111111111", email1, 11 ),
                    new Contato ( Guid.NewGuid(), new DateTime(), "Maria Silva", 35, "222222222", email2, 21 ),
                    new Contato ( Guid.NewGuid(), new DateTime(), "José Almeida", 40, "333333333", email3, 11 )
                };

                context.Contatos.AddRange(contatos);
                context.SaveChanges();

                var result = await repository.ListarContatosPorDddAsync(11);

                Assert.Equal(2, result.Count());
                Assert.All(result, c => Assert.Equal(11, c.CodigoDdd));
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task AtualizarContato_DeveAtualizarUmContatoNaDatabase()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new ContatoRepository(context);
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

                context.Contatos.Add(contato);
                context.SaveChanges();

                contato.SetNome("Pedro Silva");
                await repository.AtualizarContatoAsync(contato);

                var updatedContato = await context.Contatos.FindAsync(contato.Id);
                Assert.Equal("Pedro Silva", updatedContato.Nome);
            }
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task DeletarContato_DeveRemoverUmContatoDaDatabase()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var repository = new ContatoRepository(context);
                var email = new Email("ana.souza@example.com");

                var contato = new Contato
                (
                    Guid.NewGuid(),
                    new DateTime(),
                    "Ana Souza",
                    25,
                    "123456789",
                    email,
                    31
                );

                context.Contatos.Add(contato);
                context.SaveChanges();

                await repository.DeletarContatoAsync(contato.Id);

                var deletedContato = await context.Contatos.FindAsync(contato.Id);
                Assert.Null(deletedContato);
            }
        }
    }
}
