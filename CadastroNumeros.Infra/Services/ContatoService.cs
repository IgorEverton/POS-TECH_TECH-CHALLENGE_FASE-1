using CadastroNumeros.Infra.Interfaces.Repository;
using CadastroNumeros.Infra.Interfaces.Service;
using CadastroNumeros.Domain.Models;
using CadastroNumeros.Infra.Interfaces.Queues;
using CadastroNumeros.Domain.Configuration.Queues.RabbitMQ;

namespace CadastroNumeros.Infra.Services;

public class ContatoService : IContatoService
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IRabbitMQPublisher<Contato> _contatoPublisher;

    public ContatoService(IContatoRepository contatoRepository, IRabbitMQPublisher<Contato> publisher)
    {
        _contatoRepository = contatoRepository;
        _contatoPublisher = publisher;
    }
    public async Task<int> AtualizarContato(Contato contato)
    {
        return await _contatoRepository.AtualizarContato(contato);
    }

    public async Task<Contato> CriarContato(Contato contato)
    {
        await _contatoPublisher.PublishMessageAsync(contato, RabbitMQQueues.CadastroContatoQueue);

        return await _contatoRepository.CriarContato(contato);
    }

    public async Task DeletarContato(Guid id)
    {
        await _contatoRepository.DeletarContato(id);
    }

    public async Task<IEnumerable<Contato>> ListarContatos(int pageNumber = 1, int pageSize = 10)
    {
        return await _contatoRepository.ListarContatos(pageNumber, pageSize);
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
