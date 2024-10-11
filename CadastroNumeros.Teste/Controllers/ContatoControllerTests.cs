using CadastroNumeros.Api.Controllers;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Domain.ValueObjects;
using CadastroNumeros.Infra.Interfaces.Service;
using FluentAssertions.Equivalency;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CadastroNumeros.Teste.Controllers
{
    public class ContatoControllerTests
    {
        private readonly Mock<IContatoService> _mockService;
        private readonly ContatoController _controller;

        public ContatoControllerTests()
        {
            _mockService = new Mock<IContatoService>();
            _controller = new ContatoController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_DeveRetornarOKComOsContatos()
        {
            // Arrange
            var email1 = new Email("carlos.silva@example.com");
            var email2 = new Email("ana.souza@example.com");

            var contatos = new List<Contato>
            {
                new Contato ( Guid.NewGuid(), new DateTime(), "Carlos Silva", 28, "987654321", email1, 21 ),
                new Contato ( Guid.NewGuid(), new DateTime(), "Ana Souza", 25, "123456789", email2, 31 )
            };

            _mockService.Setup(s => s.ListarContatosAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(contatos);

            // Act
            var result = await _controller.GetAllAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Contato>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetById_DeveRetornarOKComOContato()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var email = new Email("ana.souza@example.com");
            var contato = new Contato ( contatoId, new DateTime(), "Ana Souza", 25, "123456789", email, 31 );

            _mockService.Setup(s => s.RetornarContatoAsync(contatoId)).ReturnsAsync(contato);

            // Act
            var result = await _controller.GetByIdAsync(contatoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Contato>(okResult.Value);
            Assert.Equal(contatoId, returnValue.Id);
        }

        [Fact]
        public async Task GetById_DeveRetornarNotFound()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            _mockService.Setup(s => s.RetornarContatoAsync(contatoId)).ReturnsAsync((Contato)null);

            // Act
            var result = await _controller.GetByIdAsync(contatoId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByDdd_DeveRetornarOkComOsContatos()
        {
            // Arrange
            var ddd = 21;
            var email = new Email("carlos.silva@example.com");
            var contatos = new List<Contato>
            {
                new Contato ( Guid.NewGuid(), new DateTime(),"Carlos Silva", 28, "987654321", email, 21 )
            };

            _mockService.Setup(s => s.ListarContatosPorDddAsync(ddd)).ReturnsAsync(contatos);

            // Act
            var result = await _controller.GetByIdAsync(ddd);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Contato>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task PostInserirContato_DeveRetornarCreatedAtAction()
        {
            // Arrange
            var createdContatoId = Guid.NewGuid(); // Gerar um GUID específico para o teste
            var email = new Email("carlos.silva@example.com");
            var contato = new Contato (createdContatoId, new DateTime(), "Carlos Silva", 28, "987654321", email, 21 );
            var createdContato = new Contato ( createdContatoId, new DateTime(),"Carlos Silva", 28, "987654321", email, 21 );

            _mockService.Setup(s => s.CriarContatoAsync(contato)).ReturnsAsync(createdContato);

            // Act
            var result = await _controller.PostInserirContatoAsync(contato);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<Contato>(createdAtActionResult.Value);
            Assert.Equal(createdContatoId, returnValue.Id); // Verificar se o ID retornado é o esperado
            Assert.Equal("GetByIdAsync", createdAtActionResult.ActionName); // Verificar o nome da ação
            Assert.Equal(createdContatoId, createdAtActionResult.RouteValues["id"]); // Verificar o valor do ID na rota
        }


        [Fact]
        public async Task PutAtualizacaoContato_DeveRetornarOk()
        {
            // Arrange
            var email = new Email("carlos.silva@example.com");
            var contato = new Contato ( Guid.NewGuid(), new DateTime(), "Carlos Silva", 28, "987654321", email, 21 );
            var qtdLinhasAtualizadas = 1;

            _mockService.Setup(s => s.RetornarContatoAsync(contato.Id)).ReturnsAsync(contato);
            _mockService.Setup(s => s.AtualizarContatoAsync(contato)).ReturnsAsync(qtdLinhasAtualizadas);

            // Act
            var result = await _controller.PutAtualizacaoContatoAsync(contato);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Contato>(okResult.Value);
            Assert.Equal(contato.Id, returnValue.Id);
        }

        [Fact]
        public async Task PutAtualizacaoContato_DeveRetornarBadRequestSeContatoNaoForEncontrado()
        {
            // Arrange
            var email = new Email("carlos.silva@example.com");
            var contato = new Contato ( Guid.NewGuid(), new DateTime(), "Carlos Silva", 28, "987654321", email, 21 );

            _mockService.Setup(s => s.RetornarContatoAsync(contato.Id)).ReturnsAsync((Contato)null);

            // Act
            var result = await _controller.PutAtualizacaoContatoAsync(contato);

            // Assert
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsType<NotFoundObjectResult>(notFoundResult);
            var value = notFoundResult.Value as string;
            var statusCode = notFoundResult.StatusCode as int?;
            Assert.Equal(404, statusCode);
            Assert.Equal("Contato não encontrado", value);
        }

        [Fact]
        public async Task DeleteCadastro_DeveRetornarNoContent()
        {
            // Arrange
            var contatoId = Guid.NewGuid();
            var email = new Email("carlos.silva@example.com");
            var contato = new Contato ( contatoId, new DateTime(),"Carlos Silva", 28, "987654321", email, 21 );

            _mockService.Setup(s => s.RetornarContatoAsync(contatoId)).ReturnsAsync(contato);
            _mockService.Setup(s => s.DeletarContatoAsync(contatoId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCadastro(contatoId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCadastro_DeveRetornarNotFoundSeContatoNaoForEncontrado()
        {
            // Arrange
            var contatoId = Guid.NewGuid();

            _mockService.Setup(s => s.RetornarContatoAsync(contatoId)).ReturnsAsync((Contato)null);

            // Act
            var result = await _controller.DeleteCadastro(contatoId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Contato não encontrado", notFoundResult.Value);
        }
    }
}
