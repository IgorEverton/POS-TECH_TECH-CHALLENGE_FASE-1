using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace CadastroNumeros.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContatoController : ControllerBase
{
    private readonly IContatoService _service;
    public ContatoController(IContatoService service)
    {
        _service = service;
    }

    [HttpGet("retornar-contatos")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _service.ListarContatos());
    }

    [HttpGet("retornar-contato/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contatoEncontrado = await _service.RetornarContato(id);

        if (contatoEncontrado != null)
        {
            return Ok(contatoEncontrado);
        }
        else
            return NotFound();
    }

    [HttpGet("retornar-contatos-por-ddd/{ddd}")]
    public async Task<IActionResult> GetById(int ddd)
    {
        var listaContatos = await _service.ListarContatosPorDdd(ddd);

        if (listaContatos != null)
            return Ok(listaContatos);
        else
            return NotFound();
    }

    [HttpPost("inserir-contato")]
    public async Task<IActionResult> PostInserirContato([FromBody] Contato contato)
    {
        if (contato == null)
            return BadRequest("Contato não pode ser nulo");
        
        try
        {
            contato.Id = new Guid();
            var contatoCriado = await _service.CriarContato(contato);
            return CreatedAtAction(nameof(GetById), new { id = contatoCriado.Id }, contato);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    [HttpPut("atualizar-contato")]
    public async Task<IActionResult> PutAtualizacaoContato([FromBody] Contato contatoAtualizado)
    {
        if (contatoAtualizado == null)
        {
            return BadRequest("Todos os dados devem ser preenchidos");
        }
        try
        {
            var contatoRecuperado = await _service.RetornarContato(contatoAtualizado.Id);
            if (contatoRecuperado != null)
            {
                await _service.AtualizarContato(contatoRecuperado);
                return Ok(contatoRecuperado);
            }
            else
            {
                return BadRequest("Contato não encontrado");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
    [HttpDelete("delete-contato/{id}")]
    public async Task<IActionResult> DeleteCadastro(Guid id)
    {
        try
        {
            var contatoEncontrado = await _service.RetornarContato(id);
            if (contatoEncontrado == null)
            {
                return NotFound("Contato não encontrado");
            }
            await _service.DeletarContato(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }
}