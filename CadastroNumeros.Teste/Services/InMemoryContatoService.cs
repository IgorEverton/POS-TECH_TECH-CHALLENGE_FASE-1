using CadastroNumeros.Domain.Models;
using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Data;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Infra.Services;
using CadastroNumeros.Teste.Stubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroNumeros.Teste.Services
{
    public class InMemoryContatoService : IDisposable
    {
        private readonly AppDbContext context;

        public InMemoryContatoService()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var opts = new DbContextOptions<AppDbContext>(dbOptions.Options.Extensions.ToDictionary(e => e.GetType()));
            context = new AppDbContext(opts);

        }
        public void Dispose() => context.Dispose();

        public class DatabaseTests : IClassFixture<InMemoryContatoService>
        {
            IContatoService contatoService;

            public DatabaseTests(InMemoryContatoService fixture)
            {
                IContatoRepository repo = new ContatoRepository(fixture.context);
                contatoService = new ContatoService(repo);
            }

            [Fact]
            //CriarContato
            public async Task TestMethod5()
            {
                //Arrange
                var contato = new ContatoStub().Get();

                //Act
                var result = await contatoService.CriarContato(contato);

                //Assert
                var contatos = await contatoService.ListarContatos();
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
                    await contatoService.CriarContato(contato);
                }

                //Act
                var result = await contatoService.ListarContatos();

                //Assert
                Assert.NotEqual(result, contatos);
                Assert.Equal(result.Count(), contatos.Count + 1);
            }

            [Fact]
            //RetornarContato
            public async Task TestMethod3()
            {
                // Arrange
                var contatoTeste = new ContatoStub().Get();
                await contatoService.CriarContato(contatoTeste);
                var contatos = await contatoService.ListarContatos();
                var contato = contatos.First();

                // Act
                var result = await contatoService.RetornarContato(contato.Id);

                // Assert
                Assert.Equal(contato, result);
            }

            [Fact]
            //AtualizarContato
            public async Task TestMethod2()
            {
                // Arrange
                var contato = new ContatoStub().Get();
                await contatoService.CriarContato(contato);
                string nomeAntigo = contato.Nome;
                contato.Nome = "novo nome";

                // Act
                await contatoService.AtualizarContato(contato);
                var result = await contatoService.RetornarContato(contato.Id);

                // Assert
                Assert.NotEqual(nomeAntigo, result.Nome);
            }

            [Fact]
            //DeletarContato
            public async Task TestMethod1()
            {
                // Arrange
                var contatos = await contatoService.ListarContatos();
                int quantidadeAnterior = contatos.Count();
                var id = contatos.Last().Id;

                // Act
                await contatoService.DeletarContato(id);
                var contatosAtuais = await contatoService.ListarContatos();
                int quantidadeAtual = contatosAtuais.Count();

                // Assert
                Assert.Equal(quantidadeAnterior - 1, quantidadeAtual);
            }
        }
    }
}
