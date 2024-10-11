using CadastroNumeros.Domain.Models;
using CadastroNumeros.Domain.ValueObjects;
using CadastroNumeros.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace CadastroNumeros.Teste.Data
{
    public class AppDbContextTests
    {
        private DbContextOptions<AppDbContext> CreateInMemoryOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void PodeInserirContatoNaDatabase()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                //var contato = new Contato
                //{
                //    Id = Guid.NewGuid(),
                //    DataCriacao = DateTime.Now,
                //    Nome = "Maria Silva",
                //    Idade = 25,
                //    Email = "maria.silva@example.com",
                //    Telefone = "987654321",
                //    CodigoDdd = 11
                //};

                var email = new Email("maria.silva@example.com");

                var contato = new Contato
                (
                    Guid.NewGuid(),
                    DateTime.Now,
                    "Maria Silva",
                    25,
                    email,
                    "987654321",
                    11
                );

                context.Contatos.Add(contato);
                context.SaveChanges();

                Assert.Equal(1, context.Contatos.Count());
                Assert.Equal("Maria Silva", context.Contatos.Single().Nome);
            }
        }

        [Fact]
        public void PodeRetornarContatoDaDatabase()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var email = new Email("joao.silva@example.com");

                var contato = new Contato
                (
                    Guid.NewGuid(),
                    DateTime.Now,
                    "João Silva",
                    30,
                    email,
                    "123456789",
                    11
                );

                context.Contatos.Add(contato);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var contato = context.Contatos.Single();
                Assert.Equal("João Silva", contato.Nome);
                Assert.Equal(30, contato.Idade);
                Assert.Equal("joao.silva@example.com", contato.Email);
                Assert.Equal("123456789", contato.Telefone);
                Assert.Equal(11, contato.CodigoDdd);
            }
        }

        [Fact]
        public void PodeAtualizarContatoNaDatabase()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var email = new Email("pedro.silva@example.com");

                var contato = new Contato
                (
                    Guid.NewGuid(),
                    DateTime.Now,
                    "Pedro Silva",
                    40,
                    email,
                    "1122334455",
                    21
                );

                context.Contatos.Add(contato);
                context.SaveChanges();

                contato.SetNome("Pedro Souza");
                context.Contatos.Update(contato);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                var contato = context.Contatos.Single();
                Assert.Equal("Pedro Souza", contato.Nome);
                Assert.Equal(40, contato.Idade);
                Assert.Equal("pedro.silva@example.com", contato.Email);
                Assert.Equal("1122334455", contato.Telefone);
                Assert.Equal(21, contato.CodigoDdd);
            }
        }

        [Fact]
        public void PodeDeletarContatoDaDatabase()
        {
            var options = CreateInMemoryOptions();

            using (var context = new AppDbContext(options))
            {
                var email = new Email("ana.silva@example.com");

                var contato = new Contato
                (
                    Guid.NewGuid(),
                    DateTime.Now,
                    "Ana Silva",
                    35,
                    email,
                    "9988776655",
                    31
                );

                context.Contatos.Add(contato);
                context.SaveChanges();

                context.Contatos.Remove(contato);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                Assert.Equal(0, context.Contatos.Count());
            }
        }
    }
}