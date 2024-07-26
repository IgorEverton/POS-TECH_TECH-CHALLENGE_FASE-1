using CadastroNumeros.Api.Controllers;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Implementations;
using CadastroNumeros.Infra.Data;
using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Infra.Services;
using CadastroNumeros.Teste.Stubs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroNumeros.Teste.Controllers
{
    public class InMemoryContatoController : IDisposable
    {
        private readonly AppDbContext context;

        public InMemoryContatoController()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var opts = new DbContextOptions<AppDbContext>(dbOptions.Options.Extensions.ToDictionary(e => e.GetType()));
            context = new AppDbContext(opts);

        }
        public void Dispose() => context.Dispose();

        public class DatabaseTests : IClassFixture<InMemoryContatoController>
        {
            ContatoController contatoController;

            public DatabaseTests(InMemoryContatoController fixture)
            {
                IContatoRepository repo = new ContatoRepository(fixture.context);
                var contatoService = new ContatoService(repo);
                contatoController = new ContatoController(contatoService);
            }

            [Fact]
            //CriarContato
            public async Task TestMethod5()
            {
                //Arrange
                var contato = new ContatoStub().Get();

                //Act
                var result = await contatoController.PostInserirContato(contato);

                //Assert
                var actionResult = Assert.IsType<IActionResult>(result);
                var expected = actionResult.As<Contato>();
                var actual = result.As<Contato>();
                Assert.Equal(expected, actual);
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
                    await contatoController.PostInserirContato(contato);
                }

                //Act
                var result = await contatoController.GetAll();
                var contentResult = result.As<List<Contato>>();

                //Assert
                Assert.NotEqual(contentResult, contatos);
                Assert.Equal(contentResult.Count(), contatos.Count + 1);
            }

            [Fact]
            //RetornarContato
            public async Task TestMethod3()
            {
                // Arrange
                var contato = new ContatoStub().Get();
                await contatoController.PostInserirContato(contato);

                // Act
                var result = await contatoController.GetById(contato.Id);

                // Assert
                var actionResult = Assert.IsType<IActionResult>(result);
                var actual = actionResult.As<Contato>();
                Assert.Equal(contato, actual);
            }

            [Fact]
            //AtualizarContato
            public async Task TestMethod2()
            {
                // Arrange
                var contato = new ContatoStub().Get();
                await contatoController.PostInserirContato(contato);
                string nomeAntigo = contato.Nome;
                contato.Nome = "novo nome";

                // Act
                await contatoController.PutAtualizacaoContato(contato);
                var result = await contatoController.GetById(contato.Id);

                // Assert
                var actionResult = Assert.IsType<IActionResult>(result);
                var actual = actionResult.As<Contato>();
                Assert.NotEqual(nomeAntigo, actual.Nome);
            }

            [Fact]
            //DeletarContato
            public async Task TestMethod1()
            {
                // Arrange
                var actionResult = await contatoController.GetAll();
                var contatos = actionResult.As<List<Contato>>();
                int quantidadeAnterior = contatos.Count();
                var id = contatos.Last().Id;

                // Act
                var deletedActionResult = await contatoController.DeleteCadastro(id);
                var actualActionResult = await contatoController.GetAll();
                var contatosAtuais = actionResult.As<List<Contato>>();
                int quantidadeAtual = contatosAtuais.Count();

                // Assert
                Assert.Equal(quantidadeAnterior - 1, quantidadeAtual);
            }
        }
    }
}
