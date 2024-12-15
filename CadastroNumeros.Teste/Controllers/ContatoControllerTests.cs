using CadastroNumeros.Api.Controllers;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

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
    public async Task GetAll_ShouldReturnOkWithListOfContatos()
    {
        // Arrange
        var contatos = new List<Contato>
        {
            new Contato { Id = Guid.NewGuid(), Nome = "Contato 1" },
            new Contato { Id = Guid.NewGuid(), Nome = "Contato 2" }
        };

        _mockService.Setup(s => s.ListarContatos(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(contatos);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(contatos, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenContatoExists()
    {
        // Arrange
        var contatoId = Guid.NewGuid();
        var contato = new Contato { Id = contatoId, Nome = "Teste" };

        _mockService.Setup(s => s.RetornarContato(contatoId)).ReturnsAsync(contato);

        // Act
        var result = await _controller.GetById(contatoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(contato, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenContatoDoesNotExist()
    {
        // Arrange
        var contatoId = Guid.NewGuid();

        _mockService.Setup(s => s.RetornarContato(contatoId)).ReturnsAsync((Contato)null);

        // Act
        var result = await _controller.GetById(contatoId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PostInserirContato_ShouldReturnOk_WhenContatoIsCreated()
    {
        // Arrange
        var contato = new Contato { Nome = "Teste", CodigoDdd = 11, Telefone = "999999999" };
        var resultMessage = new SolicitacaoResult { Sucesso = true, Mensagem = "Sucesso" };

        _mockService.Setup(s => s.CriarContato(It.IsAny<Contato>())).ReturnsAsync(resultMessage);

        // Act
        var result = await _controller.PostInserirContato(contato);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(resultMessage, okResult.Value);
    }

    [Fact]
    public async Task PostInserirContato_ShouldReturnBadRequest_WhenContatoIsNull()
    {
        // Act
        var result = await _controller.PostInserirContato(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Contato não pode ser nulo", badRequestResult.Value);
    }

    [Fact]
    public async Task PutAtualizacaoContato_ShouldReturnOk_WhenContatoIsUpdated()
    {
        // Arrange
        var contato = new Contato { Id = Guid.NewGuid(), Nome = "Atualizado" };
        var resultMessage = new SolicitacaoResult { Sucesso = true, Mensagem = "Atualizado com sucesso" };

        _mockService.Setup(s => s.AtualizarContato(contato)).ReturnsAsync(resultMessage);

        // Act
        var result = await _controller.PutAtualizacaoContato(contato);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(resultMessage, okResult.Value);
    }

    [Fact]
    public async Task PutAtualizacaoContato_ShouldReturnBadRequest_WhenContatoIsNull()
    {
        // Act
        var result = await _controller.PutAtualizacaoContato(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Todos os dados devem ser preenchidos", badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteCadastro_ShouldReturnOk_WhenContatoIsDeleted()
    {
        // Arrange
        var contatoId = Guid.NewGuid();
        var contato = new Contato { Id = contatoId };
        var resultMessage = new SolicitacaoResult { Sucesso = true, Mensagem = "Deletado com sucesso" };

        _mockService.Setup(s => s.RetornarContato(contatoId)).ReturnsAsync(contato);
        _mockService.Setup(s => s.DeletarContato(contatoId)).ReturnsAsync(resultMessage);

        // Act
        var result = await _controller.DeleteCadastro(contatoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(resultMessage, okResult.Value);
    }

    [Fact]
    public async Task DeleteCadastro_ShouldReturnNotFound_WhenContatoDoesNotExist()
    {
        // Arrange
        var contatoId = Guid.NewGuid();

        _mockService.Setup(s => s.RetornarContato(contatoId)).ReturnsAsync((Contato)null);

        // Act
        var result = await _controller.DeleteCadastro(contatoId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Contato não encontrado", notFoundResult.Value);
    }
}
