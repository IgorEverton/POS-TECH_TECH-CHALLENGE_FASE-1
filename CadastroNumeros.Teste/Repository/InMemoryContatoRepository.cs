using CadastroNumeros.Domain.Models;
using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Data;
using CadastroNumeros.Teste.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroNumeros.Teste.Repository
{
    public class InMemoryContatoRepository : IDisposable
    {
        private readonly AppDbContext context;

        public InMemoryContatoRepository()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var opts = new DbContextOptions<AppDbContext>(dbOptions.Options.Extensions.ToDictionary(e => e.GetType()));
            context = new AppDbContext(opts);

        }
        public void Dispose() => context.Dispose();

        public class DatabaseTests : IClassFixture<InMemoryContatoRepository>
        {
            ContatoRepository contatoRepository;

            public DatabaseTests(InMemoryContatoRepository fixture)
            {
                contatoRepository = new ContatoRepository(fixture.context);
            }

            [Fact]
            //CriarContato
            public async Task TestMethod5()
            {
                //Arrange
                var contato = new ContatoStub().Get();

                //Act
                var result = await contatoRepository.CriarContato(contato);

                //Assert
                var contatos = await contatoRepository.ListarContatos();
                Assert.Single(contatos);
                Assert.Equal(contatos.First(), result);
            }


            [Fact]
            //ListarContatos
            public async Task TestMethod4()
            {
                //Arrange
                var stub = new ContatoStub();
                var contatos = stub.Get(5);

                foreach (var contato in contatos)
                {
                    await contatoRepository.CriarContato(contato);
                }

                //Act
                var result = await contatoRepository.ListarContatos();

                //Assert
                Assert.NotEqual(result, contatos);
                Assert.Equal(result.Count(), contatos.Count + 1);
            }

            [Fact]
            //RetornarContato
            public async Task TestMethod3()
            {
                // Arrange
                var contatos = await contatoRepository.ListarContatos();
                var contato = contatos.First();

                // Act
                var result = await contatoRepository.RetornarContato(contato.Id);

                // Assert
                Assert.Equal(contato, result);
            }

            [Fact]
            //AtualizarContato
            public async Task TestMethod2()
            {
                // Arrange
                var contatos = await contatoRepository.ListarContatos();
                var contatoAntigo = contatos.First();
                var contatoNovo = contatoAntigo;
                contatoNovo.Nome = "novo nome";

                // Act
                await contatoRepository.AtualizarContato(contatoNovo);
                var result = await contatoRepository.RetornarContato(contatoNovo.Id);

                // Assert
                Assert.NotEqual(contatoAntigo.Nome, result.Nome);
            }

            [Fact]
            //DeletarContato
            public async Task TestMethod1()
            {
                // Arrange
                var contatos = await contatoRepository.ListarContatos();
                int quantidadeAnterior = contatos.Count();
                var id = contatos.Last().Id;

                // Act
                await contatoRepository.DeletarContato(id);
                var contatosAtuais = await contatoRepository.ListarContatos();
                int quantidadeAtual = contatosAtuais.Count();

                // Assert
                Assert.Equal(quantidadeAnterior - 1, quantidadeAtual);
            }
        }
    }
}
