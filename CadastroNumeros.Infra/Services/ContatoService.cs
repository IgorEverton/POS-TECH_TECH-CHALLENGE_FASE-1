using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Domain.Models;

namespace CadastroNumeros.Infra.Services;

public class ContatoService : IContatoService
{
    private readonly IContatoRepository _contatoRepository;

    public ContatoService(IContatoRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }
    public async Task AtualizarContato(Contato contato)
    {
        await _contatoRepository.AtualizarContato(contato);
    }

    public async Task<Contato> CriarContato(Contato contato)
    {
        return await _contatoRepository.CriarContato(contato);
    }

    public async Task DeletarContato(Guid id)
    {
        await _contatoRepository.DeletarContato(id);
    }

    public async Task<IEnumerable<Contato>> ListarContatos()
    {
        return await _contatoRepository.ListarContatos();
    }

    public async Task<IEnumerable<Contato>> ListarContatosPorDdd(int ddd)
    {
        return await _contatoRepository.ListarContatosPorDdd(ddd);
    }

    public async Task<Contato> RetornarContato(Guid id)
    {
        return await _contatoRepository.RetornarContato(id);
    }
}
