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
    public async Task<int> AtualizarContatoAsync(Contato contato)
    {
        return await _contatoRepository.AtualizarContatoAsync(contato);
    }

    public async Task<Contato> CriarContatoAsync(Contato contato)
    {
        return await _contatoRepository.CriarContatoAsync(contato);
    }

    public async Task DeletarContatoAsync(Guid id)
    {
        await _contatoRepository.DeletarContatoAsync(id);
    }

    public async Task<IEnumerable<Contato>> ListarContatosAsync(int pageNumber = 1, int pageSize = 10)
    {
        return await _contatoRepository.ListarContatosAsync(pageNumber, pageSize);
    }

    public async Task<IEnumerable<Contato>> ListarContatosPorDddAsync(int ddd)
    {
        return await _contatoRepository.ListarContatosPorDddAsync(ddd);
    }

    public async Task<Contato> RetornarContatoAsync(Guid id)
    {
        return await _contatoRepository.RetornarContatoAsync(id);
    }
}
