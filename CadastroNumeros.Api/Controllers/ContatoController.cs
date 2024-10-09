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
    public async Task<IActionResult> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var contatos = await _service.ListarContatosAsync(pageNumber, pageSize);
        return Ok(contatos);
    }

    [HttpGet("retornar-contato/{id}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var contatoEncontrado = await _service.RetornarContatoAsync(id);

        if (contatoEncontrado != null)
        {
            return Ok(contatoEncontrado);
        }
        else
            return NotFound();
    }

    [HttpGet("retornar-contatos-por-ddd/{ddd}")]
    public async Task<IActionResult> GetByIdAsync(int ddd)
    {
        var listaContatos = await _service.ListarContatosPorDddAsync(ddd);

        if (listaContatos != null)
            return Ok(listaContatos);
        else
            return NotFound();
    }

    [HttpPost("inserir-contato")]
    public async Task<IActionResult> PostInserirContatoAsync([FromBody] Contato contato)
    {
        if (contato == null)
            return BadRequest("Contato não pode ser nulo");

        try
        {
            contato.Id = Guid.NewGuid(); // Gerar um novo GUID para o contato
            var contatoCriado = await _service.CriarContatoAsync(contato);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = contatoCriado.Id }, contatoCriado); // Retornar CreatedAtAction com o ID correto
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPut("atualizar-contato")]
    public async Task<IActionResult> PutAtualizacaoContatoAsync([FromBody] Contato contatoAtualizado)
    {
        if (contatoAtualizado == null)
        {
            return BadRequest("Todos os dados devem ser preenchidos");
        }
        try
        {
            int linhasAtualizadas = await _service.AtualizarContatoAsync(contatoAtualizado);
            return (linhasAtualizadas > 0) ?
                Ok(contatoAtualizado)
                : NotFound("Contato não encontrado");
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
            var contatoEncontrado = await _service.RetornarContatoAsync(id);
            if (contatoEncontrado == null)
            {
                return NotFound("Contato não encontrado");
            }
            await _service.DeletarContatoAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

    }
}